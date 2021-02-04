namespace Sphyrnidae.Common.EmailUtilities.Models
{
    /// <summary>
    /// If specifying a particular email (will auto-lookup the "TO" address from configuration)
    /// </summary>
    public enum EmailType
    {
        /// <summary>
        /// An exception has occurred in the system
        /// </summary>
        Exception,

        /// <summary>
        /// An exception has occurred in the system, but is caught and ignored from the user perspective
        /// </summary>
        HiddenException,

        /// <summary>
        /// Something took a long time to run
        /// </summary>
        LongRunning,

        /// <summary>
        /// An http call failed causing an alert
        /// </summary>
        HttpFailure,

        /// <summary>
        /// General logging
        /// </summary>
        Logging,

        /// <summary>
        /// Not specified, so user must provide the meaningful bits (This will be assumed to be sent to customers)
        /// </summary>
        Custom
    }
}