using Sphyrnidae.Common.Encryption.Algorithms;
using Sphyrnidae.Common.Encryption.Models;
using Sphyrnidae.Common.Extensions;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Encryption
{
    /// <inheritdoc />
    /// <summary>
    /// Encrypts or decrypts a string.
    /// Note that this uses an encryption key stored in the environment
    /// </summary>
    public class EncryptionNone : EncryptionAlgorithm
    {
        public override string Id => "";
        public override byte[] Hash(string str, string salt) => str.ToByteArray();
        public override string Encrypt(string str) => str;
        public override DecryptionResponse Decrypt(string str) => new DecryptionResponse { IsCurrent = true, Value = str };
    }
}