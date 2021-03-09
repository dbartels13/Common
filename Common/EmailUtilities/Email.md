# Email {#EmailMd}

## Overview {#EmailOverviewMd}
Sending an email is the purpose of the [IEmail](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmail) interface.
To access this, you should likely utilize the [wrapper](@ref Sphyrnidae.Common.EmailUtilities.Email) as this allows for lots of overloads against the interface.
There is only 1 method on the interface, so your chosen implementation could be as simple as you'd like.
However, we recommend a great deal of configurability which is present in our implementation (and is easily applied to your custom implementation).

Interface: [IEmail](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmail)

Mock: [EmailMock](@ref Sphyrnidae.Common.EmailUtilities.EmailMock)

Wrapper: [Email](@ref Sphyrnidae.Common.EmailUtilities.Email)

Implementation: [DotNetEmail](@ref Sphyrnidae.Common.EmailUtilities.DotNetEmail)

## Implementation Customizations {#EmailImplementationMd}
The [DotNetEmail](@ref Sphyrnidae.Common.EmailUtilities.DotNetEmail) is very configurable.
There are many principles at work here, so we'll start with the [EmailType](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType).
This implementation also makes use of [IEmailSettings](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings) and [IDotNetEmailSettings](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IDotNetEmailSettings).
An email is generally made up of the following pieces of information:
1. From (email)
2. From (display name)
3. To
4. CC
5. BCC
6. Subject
7. Body

3-7 are provided via the interface (will be up to the consumer of this interface to specify all of these).
It should be up to your implementation to fill in 1-2, make any changes to the others, format the body, and actually send the message.

<h2>Email Types</h2>
Knowing the [type](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType) of email, allows you to hard-code certain things.
These "things", in the implementation, are the actual listing of recipients (eg. To).
This allows you to have a configurable list of recipients per email type.

<h2>Email Settings</h2>
The [IEmailSettings](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings) interface allows for all of the customizations.
There are several "Recipients" properties that will be the "To" listing of email address matching up with the corresponding [type](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType):
<table>
    <tr>
        <th>[Email Type](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType)
        <th>Property
    <tr>
        <td>[Exception](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType.Exception)
        <td>[ExceptionsRecipients](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.ExceptionsRecipients)
    <tr>
        <td>[HiddenException](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType.HiddenException)
        <td>[HiddenExceptionRecipients](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.HiddenExceptionRecipients)
    <tr>
        <td>[Logging](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType.Logging)
        <td>[LoggingRecipients](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.LoggingRecipients)
    <tr>
        <td>[LongRunning](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType.LongRunning)
        <td>[LongRunningRecipients](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.LongRunningRecipients)
    <tr>
        <td>[HttpFailure](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType.HttpFailure)
        <td>[HttpFailureRecipients](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.HttpFailureRecipients)
    <tr>
        <td>[Custom](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailType.Custom)
        <td>User-supplied during the call
</table>

Redirects are the ability to change who the email is being sent to (or cc/bcc).
A redirect could happen for any number of reasons, but the most common scenario is environment-specific.
In a non-production environment, you want to make sure that emails are not actually being sent out to end-users.
You can guard against this by having the following configurations retrieve their values from the [environment](@ref EnvironmentMd) or from [variables](@ref VariableMd).
1. [AllowRedirect](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.AllowRedirect): This should be false for production, but probably set to true for other environments. If this value is false, the remaining properties will be ignored. If this value is true, you will need to ensure the remaining properties are properly configured.
2. [AllowedDomains](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.AllowedDomains): This is a white-listing of domains (eg. sphyrnidaetech.com) where emails will go out normally. Anything that is not listed, will be redirected. The domains listed are not wild-carded. The exact phrase must match the ending of the provided email address to be allowed.
3. [DomainAndEmailComparison](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.DomainAndEmailComparison): This allows you to customize the behavior (eg. InvariantCultureIgnoreCase) for pattern matching the email address/domains.
4. [RedirectRecipients](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.RedirectRecipients): If an email address is not white-listed, this property will specify who will get the email instead (eg. send to your development/qa team).
5. [ShowRedirectedRecipients](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.ShowRedirectedRecipients): If an email address has been removed, setting this property will append this information to the end of the email (eg. will tell you whom should have received this were it not for redirects).

The remaining properties will also need to be filled in:
1. [Bcc](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.Bcc): If you always wish to BCC an account, you can set this. This can be useful for ensuring that emails are actually being sent and that email account can contain the full record of all emails in the system.
2. [FromEmail](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.FromEmail): The email address of the sender of all system emails (eg. noreply@sphyrnidaetech.com).
3. [FromName](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailSettings.FromName): The display name for the 'from' email (eg. The name of your company).

In my implementations, I have these filled in by another interface implementation: [IEmailDefaultSettings](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IEmailDefaultSettings)

<h2>Dot Net Settings</h2>
The last piece of configurability is specific to the [DotNetEmail](@ref Sphyrnidae.Common.EmailUtilities.DotNetEmail) implementation.
This implementation uses the [IDotNetEmailSettings](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IDotNetEmailSettings) to specify customizations that are specific for sending emails in .Net.
Hopefully these properties are self-explanatory, and please note the [Password](@ref Sphyrnidae.Common.EmailUtilities.Interfaces.IDotNetEmailSettings.Password) property is [encrypted](@ref EncryptionMd).

## Custom Implementation {#EmailCustomMd}
If you are developing your own implementation of sending emails, you may wish to utilize this same type of behavior.
The [EmailHelper](@ref Sphyrnidae.Common.EmailUtilities.EmailHelper) class can help apply all of these behaviors to your interface implementation.
From there, it will be your responsibility in your implementation to take everything from the [EmailItems](@ref Sphyrnidae.Common.EmailUtilities.Models.EmailItems) and put this data into your email object.

## Where Used {#EmailWhereUsedMd}
1. [SingalR](@ref SignalRMd): Errors will be emailed
2. [EmailBase](@ref Sphyrnidae.Common.EmailUtilities.EmailBase): How to send the customized email content
3. [Logger](@ref LogginMd): Any failures during logging will be emailed
4. [Settings](@ref SettingsMd): Any errors retrieving these listings will be emailed

## Examples {#EmailExampleMd}
<pre>
    public class DotNetEmail : IEmail
    {
        private IEmailSettings Settings { get; }
        private IDotNetEmailSettings DotNetSettings { get; }
        private IEncryption Encryption { get; }
        public DotNetEmail(IEmailSettings settings, IDotNetEmailSettings dotNetSettings, IEncryption encryption)
        {
            Settings = settings;
            DotNetSettings = dotNetSettings;
            Encryption = encryption;
        }

        public async Task<bool> SendAsync(EmailType type, IEnumerable<string> to, IEnumerable<string> cc, string subject, string content)
        {
            var emailInformation = EmailHelper.ToEmailItems(Settings, type, to, cc, subject, content);

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
                EnableSsl = DotNetSettings.EnableSsl
            };

            var host = DotNetSettings.Host;
            if (!string.IsNullOrWhiteSpace(host))
                client.Host = host;

            var port = DotNetSettings.Port;
            if (port.HasValue && port.Value != 0)
                client.Port = port.Value;

            var password = DotNetSettings.Password;
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
            catch (Exception ex)
            {
                return false;
            }
        }
    }
</pre>
