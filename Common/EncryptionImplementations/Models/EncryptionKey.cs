namespace Sphyrnidae.Common.EncryptionImplementations.Models
{
    /// <summary>
    /// An encryption key
    /// </summary>
    public class EncryptionKey
    {
        /// <summary>
        /// The Unique ID of the key
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The actual key value
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// If this is the current key (used for encryption, or to tell if the decryption needs updated)
        /// </summary>
        public bool IsCurrent { get; set; }
    }
}