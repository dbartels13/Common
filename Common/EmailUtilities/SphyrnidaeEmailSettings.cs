using System;
using System.Collections.Generic;
using System.Linq;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.EmailUtilities.Models;
using Sphyrnidae.Common.Extensions;
using Sphyrnidae.Common.Variable;
using Sphyrnidae.Common.Variable.Interfaces;

namespace Sphyrnidae.Common.EmailUtilities
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of IEmailSettings which populates from EmailDefaultSettings and VariableAPI
    /// </summary>
    public class SphyrnidaeEmailSettings : IEmailSettings
    {
        protected IVariableServices Variable { get; }
        protected IEmailDefaultSettings Defaults { get; }

        public SphyrnidaeEmailSettings(IVariableServices variable, IEmailDefaultSettings defaults)
        {
            Variable = variable;
            Defaults = defaults;
        }

        #region Email Type Recipients
        /// <inheritdoc />
        /// <summary>
        /// Retrieves its value from the variable API with a key of "Email_Exceptions" (Splits into array on ';' character)
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        public IEnumerable<string> ExceptionsRecipients => AddDefaultEmail("Email_Exceptions");

        /// <inheritdoc />
        /// <summary>
        /// Retrieves its value from the variable API with a key of "Email_HiddenExceptions" (Splits into array on ';' character)
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        public IEnumerable<string> HiddenExceptionRecipients => AddDefaultEmail("Email_HiddenExceptions");

        /// <inheritdoc />
        /// <summary>
        /// Retrieves its value from the variable API with a key of "Email_Logging" (Splits into array on ';' character)
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        public IEnumerable<string> LoggingRecipients => AddDefaultEmail("Email_Logging");

        /// <inheritdoc />
        /// <summary>
        /// Retrieves its value from the variable API with a key of "Email_LongRunning" (Splits into array on ';' character)
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        public IEnumerable<string> LongRunningRecipients => AddDefaultEmail("Email_LongRunning");

        /// <inheritdoc />
        /// <summary>
        /// Retrieves its value from the variable API with a key of "Email_HttpFailure" (Splits into array on ';' character)
        /// </summary>
        /// <returns>All "to" email recipients</returns>
        public IEnumerable<string> HttpFailureRecipients => AddDefaultEmail("Email_HttpFailure");
        #endregion

        #region Email Replacements
        private string Domains() => (SettingsVariable.Get(Variable, "Email_AllowedDomains", "*") ?? "").Trim();

        /// <inheritdoc />
        /// <summary>
        /// If variable API "Email_AllowedDomains" has a value of "*", then this is false (Default if not provided). Otherwise, true (enabled)
        /// </summary>
        /// <returns>True if redirect/replacements will be done, false otherwise</returns>
        public bool AllowRedirect => !Domains().Equals("*", DomainAndEmailComparison);

        /// <inheritdoc />
        /// <summary>
        /// Retrieves its value from the variable API with a key of "Email_RedirectTo" (Splits into array on ';' character)
        /// </summary>
        /// <returns>All recipients that will replace the invalid ones</returns>
        public IEnumerable<string> RedirectRecipients => AddDefaultEmail("Email_RedirectTo");

        /// <inheritdoc />
        /// <summary>
        /// Domains that will NOT be redirected to the "Redirect" list
        /// </summary>
        /// <returns>All domains which are allowed (will not send to "redirect")</returns>
        public IEnumerable<string> AllowedDomains
        {
            get
            {
                var domains = Domains();
                if (domains == "" || domains == "*")
                    return new List<string>();

                return domains.SafeSplit(";", true);
            }
        }

        /// <summary>
        /// If we are redirecting some e-mails, this will possibly show the listing of e-mails removed in the body of the email
        /// </summary>
        /// <param name="type">The type of email</param>
        /// <returns>True if this list should be in the body, false otherwise</returns>
        public bool ShowRedirectedRecipients(EmailType type) => type != EmailType.Custom;
        #endregion

        #region From/BCC
        /// <inheritdoc />
        /// <summary>
        /// Retrieves its value from the variable API with a key of "Email_Bcc" (Splits into array on ';' character)
        /// </summary>
        /// <returns>All "bcc" email recipients</returns>
        public IEnumerable<string> Bcc => GetRecipients("Email_Bcc");

        /// <inheritdoc />
        /// <summary>
        /// Retrieves its value from EmailDefaultSettings
        /// </summary>
        /// <returns>"from" email address</returns>
        public string FromEmail => Defaults.From;

        /// <inheritdoc />
        /// <summary>
        /// Retrieves its value from EmailDefaultSettings
        /// </summary>
        /// <returns>"from" email name/alias</returns>
        public string FromName => Defaults.FromName;

        /// <inheritdoc />
        /// <summary>
        /// Hard-coded to be StringComparison.CurrentCultureIgnoreCase
        /// </summary>
        public StringComparison DomainAndEmailComparison => StringComparison.CurrentCultureIgnoreCase;

        #endregion

        #region Helpers
        private IEnumerable<string> GetRecipients(string dynamicKey)
        {
            var recipients = SettingsVariable.Get(Variable, dynamicKey, "");
            return recipients.SafeSplit(";", true);
        }

        private IEnumerable<string> AddDefaultEmail(IReadOnlyCollection<string> emails)
            => emails.Count == 0 ? Defaults.To.SafeSplit(";", true) : emails;

        private IEnumerable<string> AddDefaultEmail(string dynamicKey)
            => AddDefaultEmail(GetRecipients(dynamicKey).ToList());

        #endregion
    }
}
