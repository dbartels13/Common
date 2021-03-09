using Microsoft.AspNetCore.Hosting;
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Sphyrnidae.Common.Application
{
    /// <inheritdoc />
    public abstract class SphyrnidaeApplicationSettings : IApplicationSettings
    {
        protected IWebHostEnvironment WebHost { get; }
        public SphyrnidaeApplicationSettings(IWebHostEnvironment webHost) => WebHost = webHost;

        public abstract string Name { get; }
        public abstract string Description { get; }
        public virtual string ContactName => "Doug Bartels";
        public virtual string ContactEmail => "doug@bartelsfamily.net";
        public virtual string Environment => WebHost.EnvironmentName;
    }
}