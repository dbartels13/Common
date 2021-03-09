using Sphyrnidae.Common.Encryption.KeyManager;
using System.Collections.Generic;

namespace Sphyrnidae.Common.Encryption.Algorithms
{
    /// <inheritdoc />
    public class SphyrnidaeEncryptionAlgorithms : IEncryptionAlgorithms
    {
        protected IEncryptionKeyManager Manager { get; }
        public SphyrnidaeEncryptionAlgorithms(IEncryptionKeyManager manager) => Manager = manager;

        public virtual List<EncryptionAlgorithm> All
            => new List<EncryptionAlgorithm> { Current, new EncryptionNormal(Manager), new EncryptionStrong(Manager) };

        public virtual EncryptionAlgorithm Current
            => new EncryptionWeak(Manager);

        public virtual EncryptionAlgorithm Void => new EncryptionOld(Manager);
    }
}
