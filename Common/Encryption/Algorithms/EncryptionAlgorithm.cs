using Sphyrnidae.Common.Encryption.Models;

namespace Sphyrnidae.Common.Encryption.Algorithms
{
    /// <inheritdoc />
    /// <summary>
    /// An encryption algorithm is still an IEncryption, but allows for rotation
    /// </summary>
    public abstract class EncryptionAlgorithm : IEncryption
    {
        /// <summary>
        /// The unique ID of the algorithm
        /// </summary>
        public abstract string Id { get; }

        /// <inheritdoc />
        public abstract byte[] Hash(string str, string salt);

        /// <inheritdoc />
        public bool HashMatch(string str, string salt, byte[] hash) => false; // Not to be used directly

        /// <inheritdoc />
        public abstract string Encrypt(string str);

        /// <inheritdoc />
        public abstract DecryptionResponse Decrypt(string str);
    }
}