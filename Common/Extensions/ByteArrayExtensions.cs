using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Extensions
{
    /// <summary>
    /// Extension methods for converting byte array to generic objects
    /// </summary>
    public static class ByteArrayExtensions
    {
        #region Generic conversion

        /// <summary>
        /// Converts an object to a byte[]
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="obj">The object being converted</param>
        /// <returns>The byte[] (or null)</returns>
        public static byte[] ToByteArray<T>(this T obj)
        {
            if (obj == null)
                return null;

            var bf = new BinaryFormatter();
            using var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        /// <summary>
        /// Converts a byte[] into an object
        /// </summary>
        /// <typeparam name="T">The type of object</typeparam>
        /// <param name="bytes">The byte[] being converted</param>
        /// <returns>The object (or default)</returns>
        public static T FromByteArray<T>(this byte[] bytes)
        {
            if (bytes == null)
                return default;

            var bf = new BinaryFormatter();
            using var ms = new MemoryStream(bytes);
            return (T) bf.Deserialize(ms);
        }

        #endregion

        #region String Conversions

        /// <summary>
        /// Converts a byte[] into a string
        /// </summary>
        /// <param name="bytes">The byte[] being converted</param>
        /// <returns>The string</returns>
        public static string AsString(this byte[] bytes)
        {
            var utf = new UnicodeEncoding();
            return utf.GetString(bytes);
        }

        /// <summary>
        /// Converts a string into a byte[]
        /// </summary>
        /// <param name="s">The string being converted</param>
        /// <returns>The byte[]</returns>
        public static byte[] ToBytes(this string s)
        {
            var utf = new UnicodeEncoding();
            return utf.GetBytes(s);
        }

        #endregion
    }
}
