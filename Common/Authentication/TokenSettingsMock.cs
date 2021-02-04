using Sphyrnidae.Common.Authentication.Interfaces;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.Authentication
{
    /// <inheritdoc />
    public class TokenSettingsMock : ITokenSettings
    {
        public virtual int TokenExpirationMinutes => 60;
    }
}