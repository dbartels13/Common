using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Extension methods for the Stream class
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Converts the stream to a string
        /// </summary>
        /// <param name="s">The stream to be converted</param>
        /// <returns>The string value of the stream</returns>
        public static async Task<string> AsStringAsync(this Stream s)
        {
            string str;
            s.Position = 0;
            using (var reader = new StreamReader(
                s,
                encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                bufferSize: 1024,
                leaveOpen: true))
            {
                str = await ReadStreamAsync(reader);
                // Do some processing with body…

                // Reset the request body stream position so the next middleware can read it
                s.Position = 0;
            }

            return str;
            /*
            //using var sr = new StreamReader(s);
            //return sr.ReadToEnd();
            //return new StreamReader(s).ReadToEnd();

            s.Position = 0;
            //s.Seek(0, SeekOrigin.Begin);

            // Don't wrap the StreamReader creation in a using statement though or it will close the underlying body stream at the conclusion of the using block and code later in the request lifecycle wont be able to read the body.
            // https://stackoverflow.com/questions/43403941/how-to-read-asp-net-core-response-body/43404745#43404745
            //using (var reader = new StreamReader(s))
            //{
            //    var str = reader.ReadToEnd();
            //    s.Seek(0, SeekOrigin.Begin);
            //    return str;
            //}
            var str = new StreamReader(s).ReadToEnd();
            s.Position = 0;
            //s.Seek(0, SeekOrigin.Begin);
            return str;
            */
        }

        private static Task<string> ReadStreamAsync(TextReader reader) => reader.ReadToEndAsync();
    }
}
