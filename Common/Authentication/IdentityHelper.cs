using System;
using Sphyrnidae.Common.Authentication.Interfaces;
using Sphyrnidae.Common.EncryptionImplementations;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.HttpClient;
using Sphyrnidae.Common.Serialize;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Authentication
{
    /// <summary>
    /// Methods for managing the Identity (Jwt)
    /// </summary>
    public static class IdentityHelper
    {
        #region Converting Identity => Jwt
        /// <summary>
        /// Takes an existing identity and converts it to a jwt
        /// </summary>
        /// <param name="identity">The already set/verified identity</param>
        /// <param name="token">The implementation of the ITokenSettings interface</param>
        /// <param name="encryption">The implementation of the IEncryption interface</param>
        /// <returns>The jwt</returns>
        public static string ToJwt(this SphyrnidaeIdentity identity, ITokenSettings token, IEncryption encryption)
        {
            identity.Expires = DateTime.UtcNow.AddMinutes(token.TokenExpirationMinutes);
            return identity.SerializeJson().Encrypt(encryption);
        }
        #endregion

        #region Converting Jwt => Identity
        /// <summary>
        /// Converts the jwt back into an Identity
        /// </summary>
        /// <param name="http">The implementation of the IHttpClientSettings interface</param>
        /// <param name="encryption">The implementation of the IEncryption interface</param>
        /// <returns>The identity (if valid, null otherwise)</returns>
        public static SphyrnidaeIdentity GetIdentity(IHttpClientSettings http, IEncryption encryption)
        {
            var jwt = http.Jwt;
            return GetIdentityFromJwt(encryption, jwt);
        }

        /// <summary>
        /// Parses the jwt into a useable identity and does expiration check
        /// </summary>
        /// <param name="encryption">The implementation of the IEncryption interface</param>
        /// <param name="jwt">The jwt which is encrypted as the identity</param>
        /// <returns>The identity, or null if it is somehow not valid</returns>
        public static SphyrnidaeIdentity GetIdentityFromJwt(IEncryption encryption, string jwt)
        {
            var identity = ParseJwt(encryption, jwt);
            return identity.IsExpired() ? null : identity;
        }

        /// <summary>
        /// Parses the jwt into a useable identity
        /// </summary>
        /// <param name="encryption">The implementation of the IEncryption interface</param>
        /// <param name="jwt">The jwt which is encrypted as the identity</param>
        /// <returns>The identity, or null if failed</returns>
        private static SphyrnidaeIdentity ParseJwt(IEncryption encryption, string jwt) =>
            string.IsNullOrWhiteSpace(jwt)
                ? null
                : SafeTry.IgnoreException(() => jwt
                    .Decrypt(encryption)
                    .Value
                    .DeserializeJson<SphyrnidaeIdentity>());

        private static bool IsExpired(this SphyrnidaeIdentity identity)
            => identity == null || identity.Expires < DateTime.UtcNow;
        #endregion

        #region Identity Error Reason
        /// <summary>
        /// For debugging/logging, gives a reason why you could not retrieve a jwt
        /// </summary>
        /// <param name="http">The implementation of the IHttpClientSettings interface</param>
        /// <param name="encryption">The implementation of the IEncryption interface</param>
        /// <returns>The rationale for no identity returned</returns>
        public static string RetrieveIdentityError(IHttpClientSettings http, IEncryption encryption)
            => RetrieveIdentityErrorFromJwt(encryption, http.Jwt);

        /// <summary>
        /// For debugging/logging, gives a reason why you could not retrieve a jwt
        /// </summary>
        /// <param name="encryption">The implementation of the IEncryption interface</param>
        /// <param name="jwt">The jwt which is encrypted as the identity</param>
        /// <returns>The rationale for no identity returned</returns>
        public static string RetrieveIdentityErrorFromJwt(IEncryption encryption, string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                return "No JWT provided";
            var decrypted = SafeTry.IgnoreException(() => jwt.Decrypt(encryption));
            if (string.IsNullOrWhiteSpace(decrypted?.Value))
                return "Unable to decrypt JWT";
            var identity = SafeTry.IgnoreException(() => decrypted.Value.DeserializeJson<SphyrnidaeIdentity>());
            if (identity.IsDefault())
                return "Unable to deserialize";
            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (identity.IsExpired())
                return "JWT has expired";
            return "Valid identity JWT";
        }
        #endregion
    }
}
