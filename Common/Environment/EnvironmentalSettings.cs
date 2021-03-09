using Microsoft.Extensions.Configuration;

namespace Sphyrnidae.Common.Environment
{
    /// <inheritdoc />
    public class EnvironmentalSettings : IEnvironmentSettings
    {
        protected IConfiguration Config { get; }
        public EnvironmentalSettings(IConfiguration config) => Config = config;

        /// <inheritdoc />
        public virtual string Get(string name) => Config[name];
    }
}