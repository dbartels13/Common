using Microsoft.AspNetCore.Hosting;
using Sphyrnidae.Common.Application;
using Sphyrnidae.Common.EmailUtilities.Interfaces;

namespace Sphyrnidae.Common.EmailUtilities
{
    /// <inheritdoc />
    public class EmailServices : IEmailServices
    {
        public IEmailSettings Settings { get; }
        public IDotNetEmailSettings DotNetSettings { get; }
        public IEmail Email { get; }
        public IWebHostEnvironment WebHost { get; }
        public IApplicationSettings App { get; }

        public EmailServices(
            IEmailSettings settings,
            IDotNetEmailSettings dotNetSettings,
            IEmail email,
            IWebHostEnvironment webHost,
            IApplicationSettings app)
        {
            Settings = settings;
            DotNetSettings = dotNetSettings;
            Email = email;
            WebHost = webHost;
            App = app;
        }
    }
}
