using Sphyrnidae.Common.Encryption.KeyManager.Models;

namespace Sphyrnidae.Common.Encryption.KeyManager
{
    /// <summary>
    /// Retrieves the key information required for encryption
    /// </summary>
    public interface IEncryptionKeyManager
    {
        /// <summary>
        /// The current key to be used
        /// </summary>
        EncryptionKey CurrentKey { get; }

        /// <summary>
        /// Retrieves the encryption key which was used for encryption
        /// </summary>
        /// <param name="encrypted">The encrypted string</param>
        /// <returns>The key information for decryption</returns>
        FoundEncryptionKey GetKeyFromString(string encrypted);
    }
}