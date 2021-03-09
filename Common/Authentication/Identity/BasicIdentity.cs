using System.Collections.Generic;

namespace Sphyrnidae.Common.Authentication.Identity
{
    /// <summary>
    /// The basic identity to use (no customizations)
    /// </summary>
    public class BasicIdentity : BaseIdentity
    {
        public override Dictionary<string, string> GetCustomLoggingProperties()
            => new Dictionary<string, string>();

        public override void SetDefaultProperties() { }
    }
}
