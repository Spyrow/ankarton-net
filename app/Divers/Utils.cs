using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace app.Divers
{
    public static class Utils
    {
        public static async Task<double> Bench(Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            await Task.Run(action);
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }

        public static void WriteFullLine(string value, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value.PadRight(Console.WindowWidth - 1));
            Console.ResetColor();
        }

        public static string GeneratePassword(int lowercase, int uppercase, int numerics)
        {
            const string lowers = "abcdefghijklmnopqrstuvwxyz";
            const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string number = "0123456789";

            var random = new Random();

            var generated = "!";
            for (var i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (var i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (var i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return generated.Replace("!", string.Empty);
        }

        public static string GenerateReadableString(int length)
        {
            var r = new Random();
            string[] consonants =
            {
                "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v",
                "w", "x"
            };
            string[] vowels = {"a", "e", "i", "o", "u", "ae", "y"};
            var name = "";
            name += consonants[r.Next(consonants.Length)];
            name += vowels[r.Next(vowels.Length)];
            var
                b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < length)
            {
                name += consonants[r.Next(consonants.Length)];
                b++;
                name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return name;
        }

        public static string AsQueryString(this IEnumerable<KeyValuePair<string, object>> parameters)
        {
            var keyValuePairs = parameters as IList<KeyValuePair<string, object>> ?? parameters.ToList();
            if (!keyValuePairs.Any())
                return "";

            var builder = new StringBuilder("?");

            var separator = "";
            foreach (var kvp in keyValuePairs.Where(kvp => kvp.Value != null))
            {
                builder.AppendFormat("{0}{1}={2}", separator, WebUtility.UrlEncode(kvp.Key),
                    WebUtility.UrlEncode(kvp.Value.ToString()));

                separator = "&";
            }

            return builder.ToString();
        }
    }
}