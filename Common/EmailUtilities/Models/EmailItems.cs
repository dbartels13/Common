using System.Collections.Generic;
// ReSharper disable CommentTypo

namespace Sphyrnidae.Common.EmailUtilities.Models
{
    /// <summary>
    /// The full email in abstract format
    /// </summary>
    internal class EmailItems
    {
        /// <summary>
        /// The text name of who the email is coming from
        /// </summary>
        internal string FromName { get; set; }

        /// <summary>
        /// The email address that will send this email
        /// </summary>
        internal string FromEmail { get; set; }

        /// <summary>
        /// Who the message will be sent to
        /// </summary>
        internal List<string> To { get; set; }

        /// <summary>
        /// Who will be CC'd on the message
        /// </summary>
        internal List<string> Cc { get; set; }

        /// <summary>
        /// Who will be BCC'd on the message
        /// </summary>
        internal List<string> Bcc { get; set; }

        /// <summary>
        /// The subject of the message
        /// </summary>
        internal string Subject { get; set; }

        /// <summary>
        /// The body of the message
        /// </summary>
        internal string Body { get; set; }
    }
}