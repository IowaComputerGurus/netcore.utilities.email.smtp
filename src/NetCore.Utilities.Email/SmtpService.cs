using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace ICG.NetCore.Utilities.Email
{
    /// <summary>
    ///     Represents an SMTP service that can be used to send outbound email messages.  Internally the current concrete
    ///     implementation will utilize the MailKit/MimeKit services per the recommendation of Microsoft.
    /// </summary>
    /// <remarks>
    ///     In your project you should use this service only.
    /// </remarks>
    public interface ISmtpService
    {
        /// <summary>
        /// Returns the configured administrator email for the SMTP service
        /// </summary>
        string AdminEmail { get; }

        /// <summary>
        ///     Shortcut for sending an email to the administrator, only requiring the subject and body.
        /// </summary>
        /// <param name="subject">The message subject</param>
        /// <param name="bodyHtml">The message body</param>
        void SendMessageToAdministrator(string subject, string bodyHtml);

        /// <summary>
        /// Sends a message to the administrator as well as the additional contacts provided.
        /// </summary>
        /// <param name="ccAddressList">Additional email addresses to add to the CC line</param>
        /// <param name="subject">The email subject</param>
        /// <param name="bodyHtml">The HTML content of the email</param>
        void SendMessageToAdministrator(IEnumerable<string> ccAddressList, string subject, string bodyHtml);

        /// <summary>
        ///     Sends a message to the specified recipient, with the supplied subject and body
        /// </summary>
        /// <param name="toAddress">Who is receiving the email</param>
        /// <param name="subject">The message subject</param>
        /// <param name="bodyHtml">The message body</param>
        void SendMessage(string toAddress, string subject, string bodyHtml);

        /// <summary>
        ///     Sends a message to the specified recipient, and CC's with the supplied subject and body
        /// </summary>
        /// <param name="toAddress">Who is receiving the email</param>
        /// <param name="ccAddressList">Additional CC'ed emails</param>
        /// <param name="subject">The message subject</param>
        /// <param name="bodyHtml">The message body</param>
        void SendMessage(string toAddress, IEnumerable<string> ccAddressList, string subject, string bodyHtml);
    }

    /// <inheritdoc />
    public class SmtpService : ISmtpService
    {
        private readonly SmtpServiceOptions _serviceOptions;
        private readonly IMimeMessageFactory _mimeMessageFactory;
        private readonly IMimeKitService _mimeKitService;

        /// <inheritdoc />
        public string AdminEmail => _serviceOptions?.AdminEmail;
        
        /// <summary>
        ///     DI Capable Constructor for SMTP message delivery using MimeKit/MailKit
        /// </summary>
        /// <param name="serviceOptions"></param>
        /// <param name="mimeMessageFactory"></param>
        /// <param name="mimeKitService"></param>
        public SmtpService(IOptions<SmtpServiceOptions> serviceOptions, IMimeMessageFactory mimeMessageFactory,
            IMimeKitService mimeKitService)
        {
            _serviceOptions = serviceOptions.Value;
            _mimeMessageFactory = mimeMessageFactory;
            _mimeKitService = mimeKitService;
        }

        /// <inheritdoc />
        public void SendMessageToAdministrator(string subject, string bodyHtml)
        {
            //Force to address
            SendMessage(_serviceOptions.AdminEmail, null, subject, bodyHtml);
        }

        /// <inheritdoc />
        public void SendMessageToAdministrator(IEnumerable<string> ccAddressList, string subject, string bodyHtml)
        {
            SendMessage(_serviceOptions.AdminEmail, ccAddressList, subject, bodyHtml);
        }

        /// <inheritdoc />
        public void SendMessage(string toAddress, string subject, string bodyHtml)
        {
            //Call full overload
            SendMessage(toAddress, null, subject, bodyHtml);
        }

        /// <inheritdoc />
        public void SendMessage(string toAddress, IEnumerable<string> ccAddressList, string subject, string bodyHtml)
        {
            //Convert to a mime message
            var toSend = _mimeMessageFactory.CreateFromMessage(_serviceOptions.AdminEmail, toAddress,
                ccAddressList, subject, bodyHtml);

            //Send
            _mimeKitService.SendEmail(toSend);
        }
    }
}