//using System.Collections.Generic;
//using System.Net.Http;
//using Sphyrnidae.Implementations.Helpers;
//using Sphyrnidae.Implementations.Interfaces;
//using Sphyrnidae.Utilities;
//using Sphyrnidae.Utilities.Extensions;
//using Sphyrnidae.Utilities.Interfaces;
//using Sphyrnidae.Utilities.Models;
//using SendGrid;
//using SendGrid.Helpers.Mail;

//namespace Sphyrnidae.Implementations.Common.Email
//{
//    public class SendGridEmail : IEmail
//    {
//        public bool Send(HttpRequestMessage request, EmailType type, IEnumerable<string> to, IEnumerable<string> cc, string subject, string content)
//        {
//            var emailInformation = EmailHelper.ToEmailItems(request, type, to, cc, subject, content);

//            var msg = new SendGridMessage
//            {
//                From = new EmailAddress(emailInformation.FromEmail, emailInformation.FromName),
//                Subject = emailInformation.Subject
//            };

//            foreach (var email in emailInformation.To)
//                msg.AddTo(new EmailAddress(email));
//            foreach (var email in emailInformation.Cc)
//                msg.AddCc(new EmailAddress(email));
//            foreach (var email in emailInformation.Bcc)
//                msg.AddBcc(new EmailAddress(email));

//            msg.PlainTextContent = emailInformation.Body;
//            msg.HtmlContent = emailInformation.Body.ToHtml();

//            // Will need to create interface/implementation for this
//            var settings = ServiceLocator.GetInstance<ISendGridEmailSettings>();
//            var client = new SendGridClient(settings.ApiKey());
//            // ReSharper disable once UnusedVariable
//            try
//            {
//                var response = client.SendEmailAsync(msg).Result;
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }
//    }
//}
