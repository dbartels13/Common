namespace Sphyrnidae.Common.EncryptionImplementations.Models
{
    /// <summary>
    /// The encryption key information necessary for decryption
    /// </summary>
    public class FoundEncryptionKey
    {
        /// <summary>
        /// The encrypted value that needs decrypted
        /// </summary>
        public string Encrypted { get; }

        /// <summary>
        /// If the key is the "current" key
        /// </summary>
        public bool IsCurrent { get; }

        /// <summary>
        /// The main key
        /// </summary>
        public string Key { get; }

        public FoundEncryptionKey(EncryptionKey key, string encrypted)
        {
            Encrypted = string.IsNullOrEmpty(key.Id) ? encrypted : encrypted.Remove(0, key.Id.Length);
            IsCurrent = key.IsCurrent;
            Key = key.Key;
        }
    }
}