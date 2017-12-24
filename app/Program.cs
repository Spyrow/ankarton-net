using System;
using System.Threading.Tasks;
using app.Divers;

namespace app
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            switch (args.Length)
            {
                case 3:
                {
                    var number = Convert.ToUInt16(args[0]);
                    var output = args[1];
                    var proxy = args[2];

                    await AccountsGenerator.Generate(proxy, number, output);
                    break;
                }
                case 2:
                {
                    var number = Convert.ToUInt16(args[0]);
                    var output = args[1];

                    await AccountsGenerator.Generate(number, output);
                    break;
                }
                default:
                    Utils.WriteFullLine("Usage: dotnet run [number] [output] ([proxy])", ConsoleColor.Red);
                    return;
            }

            Utils.WriteFullLine("All accounts were added successfully!", ConsoleColor.Green);
        }
    }
}