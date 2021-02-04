using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.EmailUtilities.Models;
// ReSharper disable UnusedMember.Global

namespace Sphyrnidae.Common.EmailUtilities
{
    /// <summary>
    /// Wrapper/Helper class around sending emails
    /// </summary>
    public static class Email
    {
        /// <summary>
        /// Sends a custom email
        /// </summary>
        /// <param name="services">The collection of email services</param>
        /// <param name="to">Recipient of the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        public static async Task<bool> SendAsync(IEmailServices services, string to, string subject, string content)
            => await SendAsync(services, EmailType.Custom, new List<string> {to}, null, subject, content);

        /// <summary>
        /// Sends a custom email
        /// </summary>
        /// <param name="services">The collection of email services</param>
        /// <param name="to">A collection of "to" recipients of the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        public static async Task<bool> SendAsync(IEmailServices services, IEnumerable<string> to, string subject,
            string content)
            => await SendAsync(services, EmailType.Custom, to, null, subject, content);

        /// <summary>
        /// Sends an email type that has defined/configured recipients
        /// </summary>
        /// <param name="services">The collection of email services</param>
        /// <param name="type">The type of email being sent</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        public static async Task<bool> SendAsync(IEmailServices services, EmailType type, string subject,
            string content)
            => await SendAsync(services, type, null, null, subject, content);

        /// <summary>
        /// Sends a custom email
        /// </summary>
        /// <param name="services">The collection of email services</param>
        /// <param name="to">Recipient of the email</param>
        /// <param name="cc">Who gets copied on the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        public static async Task<bool> SendAsync(IEmailServices services, string to, string cc, string subject,
            string content)
            => await SendAsync(
                services,
                EmailType.Custom,
                new List<string> {to},
                new List<string> {cc}, subject,
                content);

        /// <summary>
        /// Sends a custom email
        /// </summary>
        /// <param name="services">The collection of email services</param>
        /// <param name="to">A collection of "to" recipients of the email</param>
        /// <param name="cc">Who gets copied on the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        public static async Task<bool> SendAsync(IEmailServices services, IEnumerable<string> to, string cc,
            string subject, string content)
            => await SendAsync(services, EmailType.Custom, to, new List<string> {cc}, subject, content);

        /// <summary>
        /// Sends an email type that has defined/configured recipients
        /// </summary>
        /// <param name="services">The collection of email services</param>
        /// <param name="type">The type of email being sent</param>
        /// <param name="cc">Who gets copied on the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        public static async Task<bool> SendAsync(IEmailServices services, EmailType type, string cc, string subject,
            string content)
            => await SendAsync(services, type, null, new List<string> {cc}, subject, content);

        /// <summary>
        /// Sends a custom email
        /// </summary>
        /// <param name="services">The collection of email services</param>
        /// <param name="to">Recipient of the email</param>
        /// <param name="cc">A collection of Who gets copied on the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        public static async Task<bool> SendAsync(IEmailServices services, string to, IEnumerable<string> cc,
            string subject, string content)
            => await SendAsync(services, EmailType.Custom, new List<string> {to}, cc, subject, content);

        /// <summary>
        /// Sends a custom email
        /// </summary>
        /// <param name="services">The collection of email services</param>
        /// <param name="to">A collection of "to" recipients of the email</param>
        /// <param name="cc">A collection of Who gets copied on the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        public static async Task<bool> SendAsync(IEmailServices services, IEnumerable<string> to,
            IEnumerable<string> cc, string subject, string content)
            => await SendAsync(services, EmailType.Custom, to, cc, subject, content);

        /// <summary>
        /// Sends an email type that has defined/configured recipients
        /// </summary>
        /// <param name="services">The collection of email services</param>
        /// <param name="type">The type of email being sent</param>
        /// <param name="cc">A collection of Who gets copied on the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        public static async Task<bool> SendAsync(IEmailServices services, EmailType type, IEnumerable<string> cc,
            string subject, string content)
            => await SendAsync(services, type, null, cc, subject, content);

        private static async Task<bool> SendAsync(IEmailServices services, EmailType type, IEnumerable<string> to,
            IEnumerable<string> cc, string subject, string content)
            => await services.Email.SendAsync(services, type, to, cc, subject, content);
    }
}
