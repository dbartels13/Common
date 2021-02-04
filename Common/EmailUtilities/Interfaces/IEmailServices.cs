using Microsoft.AspNetCore.Hosting;
using Sphyrnidae.Common.Application;

namespace Sphyrnidae.Common.EmailUtilities.Interfaces
{
    /// <summary>
    /// Single injection component that wraps the injection of multiple services needed for email
    /// </summary>
    public interface IEmailServices
    {
        IEmailSettings Settings { get; }
        IDotNetEmailSettings DotNetSettings { get; }
        IEmail Email { get; }
        IWebHostEnvironment WebHost { get; }
        IApplicationSettings App { get; }
    }
}
