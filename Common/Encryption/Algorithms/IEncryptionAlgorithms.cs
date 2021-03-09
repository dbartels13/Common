using System.Collections.Generic;

namespace Sphyrnidae.Common.Encryption.Algorithms
{
    /// <summary>
    /// Allows you to "register" multiple encryption algorithms
    /// </summary>
    public interface IEncryptionAlgorithms
    {
        /// <summary>
        /// Listing of ALL possible algorithm
        /// </summary>
        List<EncryptionAlgorithm> All { get; }

        /// <summary>
        /// The current algorithm
        /// </summary>
        EncryptionAlgorithm Current { get; }

        /// <summary>
        /// The algorithm that doesn't have a key specified
        /// </summary>
        EncryptionAlgorithm Void { get; }
    }
}
