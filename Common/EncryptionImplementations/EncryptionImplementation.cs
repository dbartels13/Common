using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.EncryptionImplementations.Models;

namespace Sphyrnidae.Common.EncryptionImplementations
{
    /// <inheritdoc />
    /// <summary>
    /// An encryption implementation is still an IEncryption, but allows for rotation
    /// </summary>
    public abstract class EncryptionImplementation : IEncryption
    {
        /// <summary>
        /// The unique ID of the implementation
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