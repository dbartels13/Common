using System.Collections.Generic;
using System.Threading.Tasks;
using Sphyrnidae.Common.EmailUtilities.Models;

namespace Sphyrnidae.Common.EmailUtilities.Interfaces
{
    /// <summary>
    /// Sending E-mails
    /// </summary>
    public interface IEmail
    {
        /// <summary>
        /// Sends an E-mail
        /// </summary>
        /// <param name="services">The collection of services needed by the Email implementation</param>
        /// <param name="type">The type of email being sent (Could auto-populate the "TO")</param>
        /// <param name="to">A collection of "to" recipients of the email</param>
        /// <param name="cc">A collection of "cc" recipients of the email</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="content">Email body</param>
        /// <returns>True if send seems to be successful, false otherwise</returns>
        Task<bool> SendAsync(IEmailServices services, EmailType type, IEnumerable<string> to, IEnumerable<string> cc,
            string subject, string content);
    }
}