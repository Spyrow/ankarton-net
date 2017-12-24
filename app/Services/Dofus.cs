using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using app.Divers;
using app.Responses;

namespace app.Services
{
    public static class Dofus
    {
        private static MyProxy _proxy;
        private static readonly Mailsac Mailsac = new Mailsac();

        private static HttpClientHandler _httpClientHandler = new HttpClientHandler
        {
            UseProxy = false,
            Proxy = null
        };

        private static HttpClient _httpClient = new HttpClient(_httpClientHandler)
        {
            Timeout = TimeSpan.FromMilliseconds(10000)
        };

        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 10);

        private static readonly DataContractJsonSerializer Serializer =
            new DataContractJsonSerializer(typeof(CreateGuestResponse));

        private static MyProxy Proxy
        {
            get => _proxy;
            set
            {
                _proxy = value ?? throw new ArgumentNullException(nameof(value));
                _httpClientHandler = new HttpClientHandler
                {
                    UseProxy = true,
                    Proxy = Proxy
                };
                _httpClient = new HttpClient(_httpClientHandler)
                {
                    Timeout = TimeSpan.FromMilliseconds(10000)
                };
            }
        }

        public static async Task<string> CreateAccount(bool useOnlineProxy = true)
        {
            try
            {
                if (_proxy == null)
                {
                    if (useOnlineProxy)
                        Proxy = await ProxyHelpers.GetValidProxyOnline();
                    else
                        Proxy = await ProxyHelpers.GenerateValidProxy();
                }
            
                CreateGuestResponse guest;
                do
                {
                    guest = await CreateGuest();
                } while (guest == null);

                string account;
                do
                {
                    account = await ValidateGuest(guest.Login, guest.XPassword);
                } while (account == null);

                var result = false;
                do
                {
                    result = await ActivateEmail(account.Split(':')[0]);
                } while (result == false);

                return account;
            }
            catch (Exception e)
            {
                Utils.WriteFullLine(e.Message, ConsoleColor.Red);

                if (useOnlineProxy)
                    Proxy = await ProxyHelpers.GetValidProxyOnline();
                else
                    Proxy = await ProxyHelpers.GenerateValidProxy();

                return await CreateAccount(useOnlineProxy);
            }
        }

        private static async Task<CreateGuestResponse> CreateGuest()
        {
            Utils.WriteFullLine("Step 1/3: CREATING", ConsoleColor.DarkRed);
            await SemaphoreSlim.WaitAsync().ConfigureAwait(false);

            try
            {
                var response =
                    await _httpClient.GetAsync(
                        "https://haapi.ankama.com/json/Ankama/v2/Account/CreateGuest?game=20&lang=fr");

                if (response.IsSuccessStatusCode)
                {
                    var xpassword = response.Headers.GetValues("x-password").First();

                    var data = response.Content.ReadAsStreamAsync().Result;

                    if (Serializer.ReadObject(data) is CreateGuestResponse createGuestResponse)
                    {
                        createGuestResponse.XPassword = xpassword;

                        SemaphoreSlim.Release();
                        return createGuestResponse;
                    }
                }

                if ((int) response.StatusCode == 602)
                    throw new Exception($"IP Daily Rate Reached. ({Proxy.ProxyUri})");

                SemaphoreSlim.Release();
            }
            catch (Exception e)
            {
                SemaphoreSlim.Release();
                throw new Exception("Error CREATING: " + e.Message);
            }

            return null;
        }

        private static async Task<string> ValidateGuest(string guestLogin, string guestPassword)
        {
            Utils.WriteFullLine("Step 2/3: VALIDATING", ConsoleColor.DarkRed);
            await SemaphoreSlim.WaitAsync().ConfigureAwait(false);

            var readable = Utils.GenerateReadableString(8);
            var password = Utils.GeneratePassword(3, 2, 3);

            var parameters = new Dictionary<string, object>
            {
                {"login", readable},
                {"password", password},
                {"email", readable + "@mailsac.com"},
                {"nickname", readable + "nick"},
                {"guestLogin", guestLogin},
                {"guestPassword", guestPassword},
                {"lang", "fr"}
            }.AsQueryString();

            try
            {
                var response =
                    await _httpClient.GetAsync("https://proxyconnection.touch.dofus.com/haapi/validateGuest" +
                                               parameters);

                SemaphoreSlim.Release();

                var data = response.Content.ReadAsStringAsync().Result;

                if (data.Contains("BRUTEFORCE"))
                    throw new Exception($"Blacklisted IP By Dofus. ({Proxy.ProxyUri})");

                if (data.Contains("Votre pseudo Ankama est incorrect"))
                    throw new Exception("Votre pseudo Ankama est incorrect");

                if (response.IsSuccessStatusCode)
                    return readable + ":" + password;
            }
            catch (Exception e)
            {
                SemaphoreSlim.Release();
                throw new Exception("Error VALIDATING: " + e.Message);
            }

            return null;
        }

        private static async Task<bool> ActivateEmail(string login)
        {
            Utils.WriteFullLine("Step 3/3: ACTIVATING", ConsoleColor.DarkRed);

            await SemaphoreSlim.WaitAsync().ConfigureAwait(false);

            var messages = new List<GetMessagesResponse>();

            while (messages.Count == 0)
            {
                messages = await Mailsac.GetMessages(login + "@mailsac.com");
            }

            var mail = messages.FirstOrDefault(m => m.Subject.Contains("Validation"));
 
            var link = mail?.Links.First((l) => l.Contains("creer-un-compte"));

            if (link == null)
            {
                return false;
            }

            try
            {
                var response = await _httpClient.GetAsync(link);

                SemaphoreSlim.Release();

                if (response.IsSuccessStatusCode) return true;
            }
            catch
            {
                SemaphoreSlim.Release();
                Utils.WriteFullLine("Error Activating, retrying...");
                return false;
            }
            return false;
        }
    }
}
