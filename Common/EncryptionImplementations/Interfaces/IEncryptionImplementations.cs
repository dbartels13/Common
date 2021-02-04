using System.Collections.Generic;

namespace Sphyrnidae.Common.EncryptionImplementations.Interfaces
{
    /// <summary>
    /// Allows you to "register" multiple encryption implementations
    /// </summary>
    public interface IEncryptionImplementations
    {
        /// <summary>
        /// Listing of ALL possible implementations
        /// </summary>
        List<EncryptionImplementation> All { get; }

        /// <summary>
        /// The current implementation
        /// </summary>
        EncryptionImplementation Current { get; }

        /// <summary>
        /// The implementation that doesn't have a key specified
        /// </summary>
        EncryptionImplementation Void { get; }
    }
}
