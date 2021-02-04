using System.Collections.Generic;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;

namespace Sphyrnidae.Common.EncryptionImplementations
{
    /// <inheritdoc />
    public class SphyrnidaeEncryptionImplementations : IEncryptionImplementations
    {
        private IEncryptionKeyManager Manager { get; }
        public SphyrnidaeEncryptionImplementations(IEncryptionKeyManager manager) => Manager = manager;

        public List<EncryptionImplementation> All
            => new List<EncryptionImplementation> { Current };

        public EncryptionImplementation Current
            => new EncryptionWeak(Manager);

        public EncryptionImplementation Void => new EncryptionOld(Manager);
    }
}
