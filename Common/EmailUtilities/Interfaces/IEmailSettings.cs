using System;
using System.Collections.Generic;
using Sphyrnidae.Common.EmailUtilities.Models;

namespace Sphyrnidae.Common.EmailUtilities.Interfaces
{
    /// <summary>
    /// Interface definition for E-mails configurations
    /// </summary>
    public interface IEmailSettings
    {
        /// <summary>
        /// When sending an email of type EmailType.Exception, this will be the recipients of the email 
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        IEnumerable<string> ExceptionsRecipients { get; }
        /// <summary>
        /// When sending an email of type EmailType.HiddenException, this will be the recipients of the email 
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        IEnumerable<string> HiddenExceptionRecipients { get; }
        /// <summary>
        /// When sending an email of type EmailType.Logging, this will be the recipients of the email
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        IEnumerable<string> LoggingRecipients { get; }
        /// <summary>
        /// When sending an email of type EmailType.LongRunning, this will be the recipients of the email
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        IEnumerable<string> LongRunningRecipients { get; }
        /// <summary>
        /// When sending an email of type EmailType.HttpFailure, this will be the recipients of the email
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        IEnumerable<string> HttpFailureRecipients { get; }

        /// <summary>
        /// If true, this will replace any email going to a domain NOT listed in AllowedDomains with the Redirect list
        /// </summary>
        /// <returns>True if redirect/replacements will be done, false otherwise</returns>
        bool AllowRedirect { get; }
        /// <summary>
        /// If the email is being redirected (eg. not a valid domain), this is who gets the email instead
        /// </summary>
        /// <returns>All recipients that will replace the invalid ones</returns>
        IEnumerable<string> RedirectRecipients { get; }
        /// <summary>
        /// Domains that will NOT be redirected to the "Redirect" list
        /// </summary>
        /// <returns>All domains which are allowed (will not send to "redirect")</returns>
        IEnumerable<string> AllowedDomains { get; }
        /// <summary>
        /// If we are redirecting some e-mails, this will possibly show the listing of e-mails removed in the body of the email
        /// </summary>
        /// <param name="type">The type of email</param>
        /// <returns>True if this list should be in the body, false otherwise</returns>
        bool ShowRedirectedRecipients(EmailType type);

        /// <summary>
        /// Who will be blind copied on the email
        /// </summary>
        /// <returns>All "bcc" email recipients</returns>
        IEnumerable<string> Bcc { get; }
        /// <summary>
        /// Email address where it is sent "from"
        /// </summary>
        /// <returns>"from" email address</returns>
        string FromEmail { get; }
        /// <summary>
        /// Name/alias where it is sent "from"
        /// </summary>
        /// <returns>"from" email name/alias</returns>
        string FromName { get; }

        /// <summary>
        /// The string comparison method for determining if a domain or a user's email is provided
        /// </summary>
        /// <remarks>Recommend using IgnoreCase (eg. CurrentCultureIgnoreCase)</remarks>
        StringComparison DomainAndEmailComparison { get; }
    }
}
