using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using app.Responses;

namespace app.Services
{
    public class Mailsac
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public Mailsac(string apikey = null)
        {
            Apikey = apikey;

            _httpClient.DefaultRequestHeaders.Add("Mailsac-Key", Apikey);
        }

        private string Apikey { get; }

        public async Task<List<GetMessagesResponse>> GetMessages(string address)
        {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

            var response = await _httpClient.GetAsync("https://mailsac.com/api/addresses/" + address + "/messages");

            var serializer = new DataContractJsonSerializer(typeof(List<GetMessagesResponse>));
            var data = response.Content.ReadAsStreamAsync().Result;

            _semaphoreSlim.Release();

            if (serializer.ReadObject(data) is List<GetMessagesResponse> messagesList)
                return messagesList;

            return null;
        }

//        public async Task<string> GetLinkInEMail(string address, string mailid)
//        {
//            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);
//
//            var response = await _httpClient.GetAsync("https://mailsac.com/api/text/" + address + "/" + mailid);
//
//            var data = response.Content.ReadAsStringAsync().Result;
//
//            const string regex =
//                "(https):\\/\\/([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:\\/~+#-]*[\\w@?^=%&\\/~+#-])?";
//
//            var match = Regex.Match(data, regex);
//
//            _semaphoreSlim.Release();
//
//            return match.Success ? match.Value : null;
//        }
    }
}