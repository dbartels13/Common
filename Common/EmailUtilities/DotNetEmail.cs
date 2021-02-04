﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
using Sphyrnidae.Common.EmailUtilities.Models;
using Sphyrnidae.Common.EncryptionImplementations;
using Sphyrnidae.Common.EncryptionImplementations.Interfaces;
using Sphyrnidae.Common.Extensions;

namespace Sphyrnidae.Common.EmailUtilities
{
    /// <inheritdoc />
    public class DotNetEmail : IEmail
    {
        private IEncryption Encryption { get; }
        public DotNetEmail(IEncryption encryption) => Encryption = encryption;

        public async Task<bool> SendAsync(IEmailServices services, EmailType type, IEnumerable<string> to, IEnumerable<string> cc, string subject, string content)
        {
            var emailInformation = EmailHelper.ToEmailItems(services.Settings, type, to, cc, subject, content);

            var msg = new MailMessage
            {
                From = new MailAddress(emailInformation.FromEmail, emailInformation.FromName),
                Subject = emailInformation.Subject
            };

            foreach (var email in emailInformation.To)
                msg.To.Add(new MailAddress(email));
            foreach (var email in emailInformation.Cc)
                msg.CC.Add(new MailAddress(email));
            foreach (var email in emailInformation.Bcc)
                msg.Bcc.Add(new MailAddress(email));

            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailInformation.Body, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailInformation.Body.ToHtml(), null, MediaTypeNames.Text.Html));

            using var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = services.DotNetSettings.EnableSsl
            };

            var host = services.DotNetSettings.Host;
            if (!string.IsNullOrWhiteSpace(host))
                client.Host = host;

            var port = services.DotNetSettings.Port;
            if (port.HasValue && port.Value != 0)
                client.Port = port.Value;

            var password = services.DotNetSettings.Password;
            if (!string.IsNullOrWhiteSpace(password))
            {
                client.UseDefaultCredentials = false;
                var decrypted = password.Decrypt(Encryption).Value;
                client.Credentials = new NetworkCredential(emailInformation.FromEmail, decrypted);
            }

            // Must leave as try/catch because the client would otherwise be disposed and not captured
            try
            {
                await client.SendMailAsync(msg);
                return true;
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
                return false;
            }
        }
    }
}
