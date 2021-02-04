using Sphyrnidae.Common.Authentication.Interfaces;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.HttpClient;

namespace Sphyrnidae.Common.Authentication
{
    /// <inheritdoc />
    public class IdentityWrapper : IIdentityWrapper
    {
        protected IHttpClientSettings Http { get; }
        protected IEncryption Encryption { get; }
        public IdentityWrapper(IHttpClientSettings http, IEncryption encryption)
        {
            Http = http;
            Encryption = encryption;
        }

        private bool Set { get; set; }
        private SphyrnidaeIdentity _identity;

        public virtual SphyrnidaeIdentity Current
        {
            get
            {
                // ReSharper disable once InvertIf
                if (!Set)
                {
                    _identity = IdentityHelper.GetIdentity(Http, Encryption);
                    Set = true;
                }
                return _identity;
            }
            set
            {
                _identity = value;
                Set = true;
            }
        }
    }
}