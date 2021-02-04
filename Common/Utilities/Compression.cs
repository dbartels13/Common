using System;
using System.IO;
using System.IO.Compression;
using Sphyrnidae.Common.Extensions;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Utilities
{
    /// <summary>
    /// Compression library for byte arrays
    /// </summary>
    public static class Compression
    {
        /// <summary>
        /// Decompresses bytes
        /// </summary>
        /// <param name="data">The bytes being decompressed</param>
        /// <returns>The decompressed bytes</returns>
        public static byte[] Decompress(this byte[] data)
        {
            using var msData = new MemoryStream(data);
            using var z = new DeflateStream(msData, CompressionMode.Decompress);
            using var ms = new MemoryStream();
            z.CopyTo(ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Decompresses bytes to a string
        /// </summary>
        /// <param name="data">The bytes being decompressed</param>
        /// <returns>The decompressed string</returns>
        public static string DecompressToString(this byte[] data)
            => data.Decompress().AsString();

        /// <summary>
        /// Decompresses a base64 string to a string
        /// </summary>
        /// <param name="base64String">The base64 string being decompressed</param>
        /// <returns>The decompressed string</returns>
        public static string DecompressToString(this string base64String)
            => Convert.FromBase64String(base64String).DecompressToString();

        /// <summary>
        /// Compresses bytes
        /// </summary>
        /// <param name="data">The bytes being compressed</param>
        /// <returns>The compressed bytes</returns>
        public static byte[] Compress(this byte[] data)
        {
            using var ms = new MemoryStream();
            using var z = new DeflateStream(ms, CompressionMode.Compress, true);
            z.Write(data, 0, data.Length);
            z.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// Compresses a string to bytes
        /// </summary>
        /// <param name="s">The string being compressed</param>
        /// <returns>The compressed bytes</returns>
        public static byte[] Compress(this string s)
            => s.ToBytes().Compress();

        /// <summary>
        /// Compresses a string to base 64
        /// </summary>
        /// <param name="s">The string being compressed</param>
        /// <returns>The compressed base 64 string</returns>
        public static string CompressToBase64String(this string s)
            => Convert.ToBase64String(s.Compress());
    }
}