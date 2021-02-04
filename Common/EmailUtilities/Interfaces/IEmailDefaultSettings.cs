namespace Sphyrnidae.Common.EmailUtilities.Interfaces
{
    /// <summary>
    /// For Email, you will retrieve settings dynamically, but these will be the defaults in case those settings fail to retrieve
    /// </summary>
    public interface IEmailDefaultSettings
    {
        /// <summary>
        /// What is the default email address that Emails come from?
        /// </summary>
        /// <remarks>This should be a valid email address, but no validation is done here</remarks>
        string From { get; }

        /// <summary>
        /// What is the default email alias (name) that Emails from from?
        /// </summary>
        string FromName { get; }
        
        /// <summary>
        /// If there is no email address configured for some common things, this will be the default
        /// </summary>
        /// <remarks>This should be a valid email address, but no validation is done here</remarks>
        string To { get; }
    }
}