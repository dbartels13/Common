using Sphyrnidae.Common.Authentication.Identity;
using Sphyrnidae.Common.Encryption;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sphyrnidae.Common.Authentication.Helper
{
    /// <inheritdoc />
    public class IdentityHelper<T> : IIdentityHelper where T : BaseIdentity, new()
    {
        protected IHttpClientSettings Http { get; }
        protected IEncryption Encryption { get; }
        public IdentityHelper(IHttpClientSettings http, IEncryption encryption)
        {
            Http = http;
            Encryption = encryption;
        }

        private bool Set { get; set; }
        private BaseIdentity Identity { get; set; }
        public BaseIdentity Current {
            get
            {
                if (!Set)
                {
                    Identity = GetIdentity();
                    Set = true;
                }
                return Identity;
            }
            set
            {
                Identity = value;
                Set = true;
            }
        }

        public virtual Task<BaseIdentity> GetDefaultIdentity()
        {
            var identity = new T
            {
                Id = 1,
                Username = "Public",
                FirstName = "Default",
                LastName = "User",
                Email = "noreply@sphyrnidaetech.com",
                Roles = new List<string>()
            };
            identity.SetDefaultProperties();
            return Task.FromResult(identity as BaseIdentity);
        }

        /// <summary>
        /// JWT will expire in 20 minutes
        /// </summary>
        public virtual int ExpirationMinutes => 20;

        public virtual string ToJwt(BaseIdentity identity)
        {
            identity.Expires = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            return identity.SerializeJson().Encrypt(Encryption);
        }

        public virtual BaseIdentity GetIdentity(string jwt = null)
        {
            jwt ??= Http.Jwt;
            var identity = ParseJwt(jwt);
            return IsExpired(identity) ? null : identity;
        }

        public virtual string RetrieveIdentityErrorFromJwt(string jwt = null)
        {
            jwt ??= Http.Jwt;
            if (string.IsNullOrWhiteSpace(jwt))
                return "No JWT provided";
            var decrypted = SafeTry.IgnoreException(() => jwt.Decrypt(Encryption));
            if (string.IsNullOrWhiteSpace(decrypted?.Value))
                return "Unable to decrypt JWT";
            var identity = SafeTry.IgnoreException(() => decrypted.Value.DeserializeJson<T>());
            if (identity.IsDefault())
                return "Unable to deserialize";
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (IsExpired(identity))
                return "JWT has expired";
            return "Valid identity JWT";
        }


        #region Private methods
        protected T ParseJwt(string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                return null;

            try
            {
                return jwt
                    .Decrypt(Encryption)
                    .Value
                    .DeserializeJson<T>();
            }
            catch
            {
                return null;
            }
        }

        protected bool IsExpired(T identity)
            => identity == null || identity.Expires < DateTime.UtcNow;
        #endregion
    }
}
