using Sphyrnidae.Common.Encryption.Models;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Encryption
{
    /// <summary>
    /// Encrypts or decrypts a string.
    /// This utilizes IEncryption
    /// </summary>
    public static class EncryptionExtensions
    {
        /// <summary>
        /// Performs hashing (1-way encryption) against the string using the given salt
        /// </summary>
        /// <param name="str">The string to 1-way encrypt</param>
        /// <param name="encryption">The instance of the IEncryption interface</param>
        /// <param name="salt">A non-private/secure string used in the encryption process</param>
        /// <returns>The hashed bytes (currently 128 bytes long)</returns>
        public static byte[] Hash(this string str, IEncryption encryption, string salt) => encryption.Hash(str, salt);

        /// <summary>
        /// Hashes a string and then compares it to what is already known (eg. Does password match?)
        /// </summary>
        /// <param name="str">The string to 1-way encrypt</param>
        /// <param name="encryption">The instance of the IEncryption interface</param>
        /// <param name="salt">A non-private/secure string used in the encryption process</param>
        /// <param name="hash">The existing bytes to be compared to</param>
        /// <returns>True if the match, false otherwise</returns>
        public static bool EqualsHash(this string str, IEncryption encryption, string salt, byte[] hash)
            => encryption.HashMatch(str, salt, hash);

        /// <summary>
        /// Encrypts the given string
        /// </summary>
        /// <param name="str">The string to encrypt</param>
        /// <param name="encryption">The instance of the IEncryption interface</param>
        /// <remarks>Note that encryption failures may throw an exception, or just return null</remarks>
        /// <returns>The encrypted string</returns>
        public static string Encrypt(this string str, IEncryption encryption) => encryption.Encrypt(str);

        /// <summary>
        /// Decrypts the given string
        /// </summary>
        /// <param name="str">The string to decrypt</param>
        /// <param name="encryption">The instance of the IEncryption interface</param>
        /// <remarks>Note that decryption failures may throw an exception, or just return null value</remarks>
        /// <returns>A response object containing the decrypted value, and if the encrypted value is "current" - eg. if there is a new method/key</returns>
        public static DecryptionResponse Decrypt(this string str, IEncryption encryption) => encryption.Decrypt(str);
    }
}
