using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace ICG.NetCore.Utilities.Email
{
    /// <summary>
    ///     A factory for building MimeMessages based on user supplied inputs
    /// </summary>
    public interface IMimeMessageFactory
    {
        /// <summary>
        ///     Creates a message with the minimum required information
        /// </summary>
        /// <param name="from">The from address for the message</param>
        /// <param name="to">The to address for the message</param>
        /// <param name="subject">The subject of the message</param>
        /// <param name="bodyHtml">The HTML body contents</param>
        /// <returns></returns>
        MimeMessage CreateFromMessage(string from, string to, string subject, string bodyHtml);

        /// <summary>
        ///     Creates a message with additional CC contacts
        /// </summary>
        /// <param name="from">The from address for the message</param>
        /// <param name="to">The to address for the message</param>
        /// <param name="cc">The address(ses) to add a CC's</param>
        /// <param name="subject">The subject of the message</param>
        /// <param name="bodyHtml">The HTML body contents</param>
        /// <returns></returns>
        MimeMessage CreateFromMessage(string from, string to, IEnumerable<string> cc, string subject, string bodyHtml);

        /// <summary>
        ///  Creates a message with an attachment
        /// </summary>
        /// <param name="from">The from address for the message</param>
        /// <param name="to">The to address for the message</param>
        /// <param name="cc">The address(ses) to add a CC's</param>
        /// <param name="subject">The subject of the message</param>
        /// <param name="fileContent">Attachment Content</param>
        /// <param name="fileName">Attachment file name</param>
        /// <param name="bodyHtml">The HTML body contents</param>
        /// <returns></returns>
        MimeMessage CreateFromMessageWithAttachment(string fromAddress, string toAddress, IEnumerable<string> cc,
            string subject, byte[] fileContent,
            string fileName, string bodyHtml);
    }

    /// <inheritdoc />
    public class MimeMessageFactory : IMimeMessageFactory
    {
        private readonly ILogger _logger;

        /// <summary>
        ///     Default constructor with DI
        /// </summary>
        /// <param name="factory">A logger factory for debug logging</param>
        public MimeMessageFactory(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger<MimeMessageFactory>();
        }

        /// <inheritdoc />
        public MimeMessage CreateFromMessage(string from, string to, string subject, string bodyHtml)
        {
            return CreateFromMessage(from, to, null, subject, bodyHtml);
        }

        /// <inheritdoc />
        public MimeMessage CreateFromMessage(string from, string to, IEnumerable<string> cc, string subject, string bodyHtml)
        {
            //Validate inputs
            if (string.IsNullOrEmpty(from))
                throw new ArgumentNullException(nameof(from));
            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException(nameof(to));
            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject));
            if (string.IsNullOrEmpty(bodyHtml))
                throw new ArgumentNullException(nameof(bodyHtml));

            //Convert
            var toSend = new MimeMessage();
            toSend.From.Add(new MailboxAddress(from));
            toSend.To.Add(new MailboxAddress(to));

            //Add CC's if needed
            if (cc != null)
                foreach (var item in cc)
                    try
                    {
                        toSend.Cc.Add(new MailboxAddress(item));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error adding {item} to email copy list");
                    }

            toSend.Subject = subject;
            var bodyBuilder = new BodyBuilder {HtmlBody = bodyHtml};
            toSend.Body = bodyBuilder.ToMessageBody();
            return toSend;
        }

        public MimeMessage CreateFromMessageWithAttachment(string fromAddress, string toAddress, IEnumerable<string> cc, string subject, byte[] fileContent,
            string fileName, string bodyHtml)
        {
            //Validate inputs
            if (string.IsNullOrEmpty(fromAddress))
                throw new ArgumentNullException(nameof(fromAddress));
            if (string.IsNullOrEmpty(toAddress))
                throw new ArgumentNullException(nameof(toAddress));
            if (fileContent == null)
                throw new ArgumentNullException(nameof(fileContent));

            //Convert
            var toSend = new MimeMessage();
            toSend.From.Add(new MailboxAddress(fromAddress));
            toSend.To.Add(new MailboxAddress(toAddress));
            toSend.Subject = subject;
            //Add CC's if needed
            if (cc != null)
                foreach (var item in cc)
                    try
                    {
                        toSend.Cc.Add(new MailboxAddress(item));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error adding {item} to email copy list");
                    }
            var bodyBuilder = new BodyBuilder { HtmlBody = bodyHtml };
            bodyBuilder.Attachments.Add(fileName, fileContent);
            toSend.Body = bodyBuilder.ToMessageBody();
            return toSend;
        }
    }
}