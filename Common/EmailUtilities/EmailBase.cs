using System;
using System.Threading.Tasks;
using Sphyrnidae.Common.Authentication.Helper;
using Sphyrnidae.Common.EmailUtilities.Interfaces;
// ReSharper disable UnusedMember.Global
// ReSharper disable StringLiteralTypo

namespace Sphyrnidae.Common.EmailUtilities
{
    /// <summary>
    /// Base class for sending system emails
    /// </summary>
    /// <remarks>
    /// To send a system email, inherit from this class and implement the abstract methods
    /// </remarks>
    public abstract class EmailBase
    {
        protected IIdentityHelper Identity { get; }
        protected IEmail EmailImpl { get; }
        protected EmailBase(IIdentityHelper identity, IEmail email)
        {
            Identity = identity;
            EmailImpl = email;
        }

        /// <summary>
        /// Subject (and HTML title) of the email
        /// </summary>
        protected abstract string Subject { get; }

        /// <summary>
        /// Email-specific content
        /// </summary>
        protected abstract string Content { get; }

        /// <summary>
        /// Name of the user (If not provided, will default to identity name)
        /// </summary>
        protected virtual string Name { get; set; }

        /// <summary>
        /// E-mail address of the user (If not provided, will default to identity email address)
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Child classes must be able to specify if all the required properties have been set or not
        /// </summary>
        protected abstract bool ValidatePropertiesSet { get; }

        public void SetName(string first, string last) => Name = $"{first} {last}";

        /// <summary>
        /// Sends the email
        /// </summary>
        /// <remarks>If 'Name' or 'Email' properties are not set, this will use the logged in user</remarks>
        /// <returns>True if email seems to have sent ok, false otherwise</returns>
        public async Task<bool> SendAsync()
        {
            if (!ValidatePropertiesSet)
                throw new Exception("Failed to set properties for Email");

            var identity = Identity.Current;
            if (Name == null)
                Name = identity.FirstName + " " + identity.LastName;

            if (Email == null)
                Email = identity.Email;

            var body = Shell.Replace("\n", ""); // Remove everything that would convert to <br>

            // There is no proper email overload, so call this directly
            return await EmailUtilities.Email.SendAsync(EmailImpl, Email, Subject, body);
        }

        protected virtual string Shell => $@"
<html>
<head>
    <title>{Subject}</title>
</head>
<body style=""padding: 5px;"">
    <div style=""width: 100%; height: 60px; background-color: #2fffff; text-align: center; font-size: 40px; line-height: 50px;"">SphyrnidaeTech</div>
    <div style=""width: 100%; padding: 20px;"">
        <div style=""width: 100%"">
            {Name},
            <p>
                {Content}
            </p>
        </div>

        <div style=""width: 100%"">
            Thank you,<br />
            SphyrnidaeTech Admin
            <p>
                <i>This is an auto-generated email coming from an unmonitored account. Please do not reply to this email.</i>
            </p>
        </div>
    </div>
</body>
</html>";
    }
}
