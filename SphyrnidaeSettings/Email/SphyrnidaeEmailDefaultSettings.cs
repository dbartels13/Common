using Sphyrnidae.Common.EmailUtilities.Interfaces;

namespace Sphyrnidae.Settings.Email
{
    /// <inheritdoc />
    public class SphyrnidaeEmailDefaultSettings : IEmailDefaultSettings
    {
        public string From => "noreply@bartelsfamily.net";
        public string FromName => "Sphyrnidae Tech";
        public string To => "doug@bartelsfamily.net";
    }
}