using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using app.Divers;
using app.Services;

namespace app
{
    public static class AccountsGenerator
    {
        public static async Task Generate(string proxyPath, ushort number, string output)
        {   
            var stopwatch = Stopwatch.StartNew();
            
            await ProxyHelpers.InitProxy(proxyPath);

            for (var i = 0; i < number; i++)
                if (await Generate(output, proxyPath) == false)
                    i--;
                else
                    Utils.WriteFullLine($"{i + 1} accounts added.", ConsoleColor.White);
 
            Utils.WriteFullLine("All accounts added in " + stopwatch.Elapsed.TotalSeconds + " seconds.", ConsoleColor.Blue);
        }

        public static async Task Generate(ushort number, string output)
        {
            var stopwatch = Stopwatch.StartNew();
            
            for (var i = 0; i < number; i++)
                if (await Generate(output) == false)
                    i--;
                else
                    Utils.WriteFullLine($"{i + 1} accounts added.", ConsoleColor.White);
            
            Utils.WriteFullLine("All accounts added in " + stopwatch.Elapsed.TotalSeconds + " seconds.", ConsoleColor.Blue);
        }

        private static async Task<bool> Generate(string output, string proxyPath = null)
        {
            var stopwatch = Stopwatch.StartNew();

            string account;

            if (proxyPath == null)
            {
                account = await Dofus.CreateAccount();
            }
            else
            {
                account = await Dofus.CreateAccount(false);
            }

            stopwatch.Stop();

            Utils.WriteFullLine($"Account {account.Split(':')[0]} added in {stopwatch.Elapsed.TotalSeconds} seconds.",
                ConsoleColor.White);

            return true;
        }
    }
}