using Sphyrnidae.Common.Authentication.Identity;
using System.Collections.Generic;

namespace Sphyrnidae.Settings
{
    /// <summary>
    /// Adds the property CustomerId to the identity
    /// </summary>
    public class SphyrnidaeIdentity : BaseIdentity
    {
        public const string CustomerIdKey = "CustomerId";

        /// <summary>
        /// ID for the customer
        /// </summary>
        public string CustomerId { get; set; }

        public override Dictionary<string, string> GetCustomLoggingProperties()
        {
            var dict = new Dictionary<string, string>
            {
                {CustomerIdKey, CustomerId }
            };
            return dict;
        }

        public override void SetDefaultProperties() { }
    }
}
