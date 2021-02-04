namespace Sphyrnidae.Common.EmailUtilities.Interfaces
{
    /// <summary>
    /// Interface used by DotNetEmail
    /// </summary>
    /// <remarks>
    /// You will need to implement this interface and have it dependency injected in order to use anything that consumes this interface.
    /// </remarks>
    public interface IDotNetEmailSettings
    {
        /// <summary>
        /// Name of the host which will send emails
        /// </summary>
        string Host { get; }

        /// <summary>
        /// Port for smtp connection on the host
        /// </summary>
        int? Port { get; }

        /// <summary>
        /// Encrypted password of the sending email account (IEmailDefaultSettings.From).
        /// If there is no password needed (eg. using defaultNetworkCredentials), this can be left blank
        /// </summary>
        string Password { get; }

        /// <summary>
        /// If the port/host uses SSL for SMTP
        /// </summary>
        bool EnableSsl { get; }
    }
}