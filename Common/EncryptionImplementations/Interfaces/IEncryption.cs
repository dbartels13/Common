using Sphyrnidae.Common.EncryptionImplementations.Models;

namespace Sphyrnidae.Common.EncryptionImplementations.Interfaces
{
    /// <summary>
    /// Encryption of a string
    /// </summary>
    public interface IEncryption
    {
        /// <summary>
        /// Performs hashing (1-way encryption) against the string using the given salt
        /// </summary>
        /// <param name="str">The string to 1-way encrypt</param>
        /// <param name="salt">A non-private/secure string used in the encryption process</param>
        /// <returns>The hashed bytes</returns>
        byte[] Hash(string str, string salt);

        /// <summary>
        /// Determines if the hashed string is the same as what was provided (eg. passwords match)
        /// </summary>
        /// <param name="str">The string to hash</param>
        /// <param name="salt">The hashing salt</param>
        /// <param name="hash">The hash to compare</param>
        /// <returns>True if the hashed string matches</returns>
        bool HashMatch(string str, string salt, byte[] hash);

        /// <summary>
        /// Encrypts the given string
        /// </summary>
        /// <param name="str">The string to encrypt</param>
        /// <remarks>Note that encryption failures may throw an exception, or just return null</remarks>
        /// <returns>The encrypted string</returns>
        string Encrypt(string str);

        /// <summary>
        /// Decrypts the given string
        /// </summary>
        /// <param name="str">The string to decrypt</param>
        /// <remarks>Note that decryption failures may throw an exception, or just return null value</remarks>
        /// <returns>A response object containing the decrypted value, and if the encrypted value is "current" - eg. if there is a new method/key</returns>
        DecryptionResponse Decrypt(string str);
    }
}