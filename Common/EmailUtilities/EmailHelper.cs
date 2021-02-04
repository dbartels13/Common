using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.EmailUtilities.Models;

namespace Sphyrnidae.Common.EmailUtilities
{
    internal static class EmailHelper
    {
        internal static EmailItems ToEmailItems(IEmailSettings settings, EmailType type, IEnumerable<string> to, IEnumerable<string> cc, string subject, string content)
        {
            var email = new EmailItems
            {
                // "From" comes directly from settings (no validation)
                FromName = settings.FromName,
                FromEmail = settings.FromEmail,

                // "To" is possibly set by the "Type" (or by what was passed in)
                To = (type switch
                {
                    EmailType.Exception => settings.ExceptionsRecipients,
                    EmailType.HiddenException => settings.HiddenExceptionRecipients,
                    EmailType.Logging => settings.LoggingRecipients,
                    EmailType.LongRunning => settings.LongRunningRecipients,
                    EmailType.HttpFailure => settings.HttpFailureRecipients,
                    EmailType.Custom => to,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                }).ToList()
            };

            var replacements = new StringBuilder();

            // Ensure "To" is valid
            var validation = email.To.SetEmails(settings);
            if (validation.Item1.Count == 0)
                throw new Exception("To send an email, you must specify a valid \"To\" address");
            email.To = validation.Item1;
            if (validation.Item2.Length > 0)
            {
                replacements.Append("Removed 'To': ");
                replacements.Append(validation.Item2);
                replacements.AppendLine("<br />");
            }

            // Ensure "CC" is valid
            validation = cc.SetEmails(settings);
            email.Cc = validation.Item1;
            if (validation.Item2.Length > 0)
            {
                replacements.Append("Removed 'CC': ");
                replacements.Append(validation.Item2);
                replacements.AppendLine("<br />");
            }
            email.Cc = email.Cc.RemoveDuplicates(email.To, settings.DomainAndEmailComparison);

            // Ensure "BCC" is valid
            validation = settings.Bcc.SetEmails(settings);
            email.Bcc = validation.Item1;
            if (validation.Item2.Length > 0)
            {
                replacements.Append("Removed 'BCC': ");
                replacements.Append(validation.Item2);
                replacements.AppendLine("<br />");
            }
            email.Bcc = email.Bcc.RemoveDuplicates(email.To, settings.DomainAndEmailComparison);
            email.Bcc = email.Bcc.RemoveDuplicates(email.Cc, settings.DomainAndEmailComparison);

            // Subject must be given
            if (string.IsNullOrWhiteSpace(subject))
                throw new Exception("To send an email, you must specify a subject");
            email.Subject = subject;

            // Body/content must be given
            if (string.IsNullOrWhiteSpace(content))
                throw new Exception("To send an email, you must specify some content");
            email.Body = content;
            if (replacements.Length > 0 && settings.ShowRedirectedRecipients(type))
                email.Body = $"{email.Body}<br />{replacements}";

            // All done
            return email;
        }

        private static Tuple<List<string>, string> SetEmails(this IEnumerable<string> emails, IEmailSettings settings)
        {
            // Check that something was given
            var emailList = new List<string>();
            if (emails == null)
                return new Tuple<List<string>, string>(emailList, "");

            // Get rid of blank ones
            var list = emails.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            // See if we need to do Redirects
            var replacements = new List<string>();
            if (settings.AllowRedirect)
            {
                var domains = settings.AllowedDomains.ToList();
                var recipients = settings.RedirectRecipients.ToList();
                foreach (var email in list)
                {
                    // If good domain, use the email as-is
                    if (domains.Any(domain => email.EndsWith($"@{domain}", settings.DomainAndEmailComparison)))
                    {
                        emailList.Add(email);
                        continue;
                    }

                    // Invalid domain, do replacement
                    replacements.Add(email);
                    if (replacements.Count == 0)
                        emailList.AddRange(recipients);
                }
            }
            else
                emailList.AddRange(list);

            // Get rid of any duplicates
            list = new List<string>();
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var email in emailList)
            {
                if (!list.HasEmail(email, settings.DomainAndEmailComparison))
                    list.Add(email);
            }

            // Get rid of blank ones
            emailList = list.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

            // All done
            return new Tuple<List<string>, string>(emailList, string.Join(";", replacements));
        }

        private static List<string> RemoveDuplicates(this IEnumerable<string> emails, IReadOnlyCollection<string> other,
            StringComparison comparison)
            => emails.Where(email => !other.HasEmail(email, comparison)).ToList();

        private static bool HasEmail(this IEnumerable<string> emails, string email, StringComparison comparison)
            => emails.Any(x => x.Equals(email, comparison));
    }
}
