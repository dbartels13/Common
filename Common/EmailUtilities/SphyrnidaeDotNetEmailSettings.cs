using Sphyrnidae.Common.EmailUtilities.Interfaces;
// ReSharper disable StringLiteralTypo

namespace Sphyrnidae.Common.EmailUtilities
{
    /// <inheritdoc />
    public class SphyrnidaeDotNetEmailSettings : IDotNetEmailSettings
    {
        public string Host => "smtp.hostinger.com";
        public int? Port => 587;
        public string Password => "3afcd81b-44a1-4d9c-8066-559b411d543f24a776b0-c967-4780-8010-95d2b90e456cBt7j6MroFXW5kCVIPrXQZQo3orGoQ9YrqsQcD/8gNym+FesBhcXOWYZ/Sj4Gc6H5rQ/BBIp1u/Zf/ZymbVkfadKwzNk2BlIWCPiAy50jTRI=";
        public bool EnableSsl => true;
    }
}