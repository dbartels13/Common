namespace Sphyrnidae.Common.Authentication
{
    /// <inheritdoc />
    public class SphyrnidaeTokenSettings : TokenSettings
    {
        public override int TokenExpirationMinutes => 60;
    }
}