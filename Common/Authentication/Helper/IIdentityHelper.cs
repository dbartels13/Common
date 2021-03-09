using Sphyrnidae.Common.Authentication.Identity;
using System.Threading.Tasks;

namespace Sphyrnidae.Common.Authentication.Helper
{
    /// <summary>
    /// Working with the identity/jwt
    /// </summary>
    public interface IIdentityHelper
    {
        /// <summary>
        /// The current identity - is updateable
        /// </summary>
        /// <remarks>
        /// Note that this returns the base type "BaseIdentity".
        /// You can always cast this to your proper type.
        /// </remarks>
        BaseIdentity Current { get; set; }

        /// <summary>
        /// Retrieves the 'default' identity object
        /// </summary>
        /// <remarks>
        /// Note that this returns the base type "BaseIdentity".
        /// You can always cast this to your proper type.
        /// </remarks>
        Task<BaseIdentity> GetDefaultIdentity();

        /// <summary>
        /// How many minutes before a JWT expires
        /// </summary>
        int ExpirationMinutes { get; }

        /// <summary>
        /// Takes an existing identity and converts it to a jwt
        /// </summary>
        /// <param name="identity">The already set/verified identity</param>
        /// <returns>The jwt</returns>
        string ToJwt(BaseIdentity identity);

        /// <summary>
        /// Converts the request jwt into an Identity
        /// </summary>
        /// <remarks>
        /// Note that this returns the base type "BaseIdentity".
        /// You can always cast this to your proper type.
        /// </remarks>
        /// <param name="jwt">Optional: The jwt which is encrypted as the identity. If not provided, will lookup from the request</param>
        /// <returns>The identity (if valid, null otherwise)</returns>
        BaseIdentity GetIdentity(string jwt = null);

        /// <summary>
        /// For debugging/logging, gives a reason why you could not retrieve a jwt
        /// </summary>
        /// <param name="jwt">Optional: The jwt which is encrypted as the identity. If not provided, will lookup from the request</param>
        /// <returns>The rationale for no identity returned</returns>
        string RetrieveIdentityErrorFromJwt(string jwt = null);
    }
}
