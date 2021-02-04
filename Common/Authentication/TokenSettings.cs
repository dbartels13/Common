using Sphyrnidae.Common.Authentication.Interfaces;

namespace Sphyrnidae.Common.Authentication
{
    /// <inheritdoc />
    public class TokenSettings : ITokenSettings
    {
        public virtual int TokenExpirationMinutes => 20;
    }
}