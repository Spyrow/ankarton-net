using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace app.Divers
{
    public static class AsyncIo
    {
        public static async Task WriteTextAsync(string filePath, string text)
        {
            var encodedText = Encoding.UTF8.GetBytes(text);

            using (var sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.Write,
                4096, true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            }
        }

        public static async Task<string> ReadTextAsync(string filePath)
        {
            using (var sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                4096, true))
            {
                var sb = new StringBuilder();

                var buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    var text = Encoding.UTF8.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return sb.ToString();
            }
        }
    }
}