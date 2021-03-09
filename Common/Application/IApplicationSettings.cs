namespace Sphyrnidae.Common.Application
{
    /// <summary>
    /// Interface definition for common application-wide hard-coded settings
    /// </summary>
    public interface IApplicationSettings
    {
        /// <summary>
        /// The name of your application.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description for your application.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Name of the contact person
        /// </summary>
        string ContactName { get; }

        /// <summary>
        /// Email address of the contact person
        /// </summary>
        /// <remarks>This should be a valid email address, but no validation is done here (done in swagger)</remarks>
        string ContactEmail { get; }

        /// <summary>
        /// The name of the current environment
        /// </summary>
        /// <remarks>You should pull this from IWebHostEnvironment.EnvironmentName</remarks>
        string Environment { get; }
    }
}