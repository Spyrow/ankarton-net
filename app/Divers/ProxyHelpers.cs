using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace app.Divers
{
    public static class ProxyHelpers
    {
        private static List<string> _proxyList = new List<string>();

        public static async Task InitProxy(string list = "proxy.txt")
        {
            var path = Path.Combine(Directory.GetCurrentDirectory() + $@"/{list}");
            var result = await AsyncIo.ReadTextAsync(path);
            _proxyList = new List<string>(result.Split(new[] {"\n", "\r"},
                StringSplitOptions.RemoveEmptyEntries));

           Utils.WriteFullLine("Initialized Proxy: " + _proxyList.Count, ConsoleColor.Green);
        }

        private static async Task<MyProxy> GetProxyOnline()
        {
            var client = new HttpClient();
            var proxy = await client.GetStringAsync("http://pubproxy.com/api/proxy?api=cDhCQVlKaGlTWXNlRXpLMmxYOHZDZz09&format=txt&type=http");
            return new MyProxy("http://" + proxy);
        }

        public static async Task<MyProxy> GetValidProxyOnline()
        {
            var stopwatch = Stopwatch.StartNew();

            Utils.WriteFullLine("Searching a valid proxy online ...", ConsoleColor.Yellow);

            MyProxy proxy;

            try {
              do
              {
                  proxy = await GetProxyOnline();
              } while (await ProxyTest(proxy) == null);
            }
            catch
            {
              return await GetValidProxyOnline();
            }

            Utils.WriteFullLine(
                $"Took {stopwatch.Elapsed.TotalSeconds} seconds to generate a valid proxy online. ({proxy.ProxyUri})",
                ConsoleColor.Cyan);

            return proxy;
        }

        private static async Task<MyProxy> ProxyTest(MyProxy proxy, int timeout = 10000)
        {
            return proxy;
//            try
//            {
//                var httpClientHandler = new HttpClientHandler
//                {
//                    UseProxy = true,
//                    Proxy = proxy
//                };
//
//                Utils.WriteFullLine("Testing proxy " + proxy.ProxyUri, ConsoleColor.Green);
//
//                var httpClient = new HttpClient(httpClientHandler) {Timeout = TimeSpan.FromMilliseconds(timeout)};
//
//                var response = await httpClient.GetAsync("https://www.dofus.com/fr");
//
//                return response.IsSuccessStatusCode ? proxy : null;
//            }
//            catch
//            {
//                return null;
//            }
        }

        private static async Task<MyProxy> GenerateProxy()
        {
            var number = await Randomize.GetRandomInt(0, _proxyList.Count - 1);
            var proxytxt = _proxyList[number];
            return new MyProxy("http://" + proxytxt);
        }

        public static async Task<MyProxy> GenerateValidProxy()
        {
            var stopWatch = Stopwatch.StartNew();
            Utils.WriteFullLine("Searching a valid proxy in the list ...", ConsoleColor.Yellow);

            MyProxy p;
            do
            {
                p = await GenerateProxy();
            } while (await ProxyTest(p) == null);

            Utils.WriteFullLine($"Proxy Found in {stopWatch.Elapsed.TotalSeconds} seconds. ({p.ProxyUri})", ConsoleColor.Cyan);

            return p;
        }
    }
}
