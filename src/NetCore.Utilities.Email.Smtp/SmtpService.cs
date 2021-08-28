using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace ICG.NetCore.Utilities.Email.Smtp
{
    /// <inheritdoc />
    public class SmtpService : IEmailService
    {
        private readonly SmtpServiceOptions _serviceOptions;
        private readonly IMimeMessageFactory _mimeMessageFactory;
        private readonly IMimeKitService _mimeKitService;

        /// <inheritdoc />
        public string AdminEmail => _serviceOptions?.AdminEmail;

        /// <inheritdoc />
        public string AdminName => _serviceOptions?.AdminName;
        
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
        public bool SendMessageToAdministrator(string subject, string bodyHtml)
        {
            //Force to address
            return SendMessage(_serviceOptions.AdminEmail, null, subject, bodyHtml, null, "");
        }

        /// <inheritdoc />
        public bool SendMessageToAdministrator(IEnumerable<string> ccAddressList, string subject, string bodyHtml)
        {
            return SendMessage(_serviceOptions.AdminEmail, ccAddressList, subject, bodyHtml, null, "");
        }

        /// <inheritdoc />
        public bool SendMessage(string toAddress, string subject, string bodyHtml)
        {
            //Call full overload
            return SendMessage(toAddress, null, subject, bodyHtml, null, "");
        }

        /// <inheritdoc />
        public bool SendMessage(string toAddress, string subject, string bodyHtml, List<KeyValuePair<string, string>> tokens)
        {
            return SendMessage(toAddress, null, subject, bodyHtml, null, "");
        }

        /// <inheritdoc />
        public bool SendMessage(string toAddress, IEnumerable<string> ccAddressList, string subject, string bodyHtml)
        {
            return SendMessage(toAddress, ccAddressList, subject, bodyHtml, null, "");
        }

        /// <inheritdoc />
        public bool SendMessage(string toAddress, IEnumerable<string> ccAddressList, string subject, string bodyHtml, List<KeyValuePair<string, string>> tokens)
        {
            return SendMessage(toAddress, ccAddressList, subject, bodyHtml, tokens, "");
        }

        /// <inheritdoc />
        public bool SendMessage(string toAddress, IEnumerable<string> ccAddressList, string subject, string bodyHtml,
            List<KeyValuePair<string, string>> tokens,
            string templateName, string senderKeyName = "")
        {
            //TODO: Optimize this
            if (tokens != null)
            {
                foreach (var item in tokens)
                {
                    bodyHtml = bodyHtml.Replace(item.Key, item.Value);
                }
            }

            //Convert to a mime message
            var toSend = _mimeMessageFactory.CreateFromMessage(_serviceOptions.AdminEmail, toAddress, ccAddressList,
                subject, bodyHtml, templateName);
            
            //Send
            _mimeKitService.SendEmail(toSend);

            return true; //Success
        }

        /// <inheritdoc />
        public bool SendMessageWithAttachment(string toAddress, IEnumerable<string> ccAddressList, string subject,
            byte[] fileContent, string fileName, string bodyHtml, List<KeyValuePair<string, string>> tokens, string templateName = "", string senderKeyName = "")
        {
            //TODO: Optimize this
            if (tokens != null)
            {
                foreach (var item in tokens)
                {
                    bodyHtml = bodyHtml.Replace(item.Key, item.Value);
                }
            }

            //Covert to a mime message
            var toSend = _mimeMessageFactory.CreateFromMessageWithAttachment(_serviceOptions.AdminEmail, toAddress,
                ccAddressList, subject, fileContent, fileName, bodyHtml, templateName);

            //Send
            _mimeKitService.SendEmail(toSend);

            return true;
        }
    }
}