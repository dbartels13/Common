using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Sphyrnidae.Common.Environment;
using Sphyrnidae.Common.Utilities;

namespace Sphyrnidae.Common.Api.ServiceRegistration.Models
{
    /// <summary>
    /// Health Check
    /// </summary>
    public class HealthCheck
    {
        /// <summary>
        /// Health Status
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// Currently running version of code
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Any errors in the system
        /// </summary>
        public object Errors { get; }

        public HealthCheck(HealthReport r, IEnvironmentSettings env)
        {
            Status = r.Status.ToString();
            Version = SafeTry.IgnoreException(() =>
                SettingsEnvironmental.Get(env, "version") ?? Assembly.GetEntryAssembly()?.GetName().Version.ToString(),
                "Unknown"
            );
            Errors = r.Entries.Select(e => new { key = e.Key, value = e.Value.Status.ToString() });
        }
    }
}