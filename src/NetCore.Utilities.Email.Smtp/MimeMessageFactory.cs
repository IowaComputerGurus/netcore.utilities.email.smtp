using System;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ICG.NetCore.Utilities.Email.Smtp
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
        /// <param name="fromName">The name that should be used for the sender</param>
        /// <param name="to">The to address for the message</param>
        /// <param name="subject">The subject of the message</param>
        /// <param name="bodyHtml">The HTML body contents</param>
        /// <returns></returns>
        MimeMessage CreateFromMessage(string from, string fromName, string to, string subject, string bodyHtml);

        /// <summary>
        ///     Creates a message with additional CC contacts
        /// </summary>
        /// <param name="from">The from address for the message</param>
        /// <param name="fromName">The name that should be used for the sender</param>
        /// <param name="to">The to address for the message</param>
        /// <param name="cc">The address(ses) to add a CC's</param>
        /// <param name="subject">The subject of the message</param>
        /// <param name="bodyHtml">The HTML body contents</param>
        /// <param name="templateName">The optional custom template to override with</param>
        /// <returns></returns>
        MimeMessage CreateFromMessage(string from, string fromName, string to, IEnumerable<string> cc, string subject, string bodyHtml, string templateName = "");

        /// <summary>
        ///  Creates a message with an attachment
        /// </summary>
        /// <param name="fromAddress">The from address for the message</param>
        /// <param name="fromName">The name that should be used for the sender</param>
        /// <param name="toAddress">The to address for the message</param>
        /// <param name="cc">The address(ses) to add a CC's</param>
        /// <param name="subject">The subject of the message</param>
        /// <param name="fileContent">Attachment Content</param>
        /// <param name="fileName">Attachment file name</param>
        /// <param name="bodyHtml">The HTML body contents</param>
        /// <param name="templateName">The optional custom template to override with</param>
        /// <returns></returns>
        MimeMessage CreateFromMessageWithAttachment(string fromAddress, string fromName, string toAddress, IEnumerable<string> cc,
            string subject, byte[] fileContent,
            string fileName, string bodyHtml, string templateName = "");
    }

    /// <inheritdoc />
    public class MimeMessageFactory : IMimeMessageFactory
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEmailTemplateFactory _emailTemplateFactory;
        private readonly SmtpServiceOptions _serviceOptions;
        private readonly ILogger _logger;

        /// <summary>
        ///     Default constructor with DI
        /// </summary>
        /// <param name="serviceOptions">Configuration options</param>
        /// <param name="logger">An instance of ILogger for recording</param>
        /// <param name="hostingEnvironment">Current environment information</param>
        /// <param name="emailTemplateFactory">The ICG Email Template Factory for formatting messages</param>
        public MimeMessageFactory(IOptions<SmtpServiceOptions> serviceOptions, ILogger<MimeMessageFactory> logger, IHostingEnvironment hostingEnvironment, IEmailTemplateFactory emailTemplateFactory)
        {
            _serviceOptions = serviceOptions.Value;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _emailTemplateFactory = emailTemplateFactory;
        }

        /// <inheritdoc />
        public MimeMessage CreateFromMessage(string from, string fromName, string to, string subject, string bodyHtml)
        {
            return CreateFromMessage(from, fromName, to, null, subject, bodyHtml);
        }

        /// <inheritdoc />
        public MimeMessage CreateFromMessage(string from, string fromName, string to, IEnumerable<string> cc, string subject, string bodyHtml, string templateName = "")
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
            var fromAddress = MailboxAddress.Parse(from);
            if (!string.IsNullOrEmpty(fromName))
            {
                fromAddress.Name = fromName;
            }

            toSend.From.Add(fromAddress);
            toSend.To.Add(MailboxAddress.Parse(to));

            //Add CC's if needed
            if (cc != null)
                foreach (var item in cc)
                    try
                    {
                        toSend.Cc.Add(MailboxAddress.Parse(item));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"Unable to add {item} to email copy list");
                    }

            if (_serviceOptions.AddEnvironmentSuffix && !_hostingEnvironment.IsProduction())
                toSend.Subject = $"{subject} ({_hostingEnvironment.EnvironmentName})";
            else
                toSend.Subject = subject;

            //Perform templating
            if (_serviceOptions.AlwaysTemplateEmails && string.IsNullOrEmpty(templateName))
                bodyHtml = _emailTemplateFactory.BuildEmailContent(toSend.Subject, bodyHtml);
            else if (!string.IsNullOrEmpty(templateName))
                bodyHtml = _emailTemplateFactory.BuildEmailContent(toSend.Subject, bodyHtml,
                    templateName: templateName);

            var bodyBuilder = new BodyBuilder {HtmlBody = bodyHtml};
            toSend.Body = bodyBuilder.ToMessageBody();
            return toSend;
        }

        /// <inheritdoc />
        public MimeMessage CreateFromMessageWithAttachment(string fromAddress, string fromName, string toAddress, IEnumerable<string> cc, string subject, byte[] fileContent,
            string fileName, string bodyHtml, string templateName = "")
        {
            //Validate inputs
            if (string.IsNullOrEmpty(fromAddress))
                throw new ArgumentNullException(nameof(fromAddress));
            if (string.IsNullOrEmpty(toAddress))
                throw new ArgumentNullException(nameof(toAddress));
            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException(nameof(subject));
            if (fileContent == null)
                throw new ArgumentNullException(nameof(fileContent));
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));
            if (string.IsNullOrEmpty(bodyHtml))
                throw new ArgumentNullException(nameof(bodyHtml));

            //Convert
            var toSend = new MimeMessage();
            var from = MailboxAddress.Parse(fromAddress);
            if (!string.IsNullOrEmpty(fromName))
            {
                from.Name = fromName;
            }

            toSend.From.Add(from);
            toSend.To.Add(MailboxAddress.Parse(toAddress));

            if (_serviceOptions.AddEnvironmentSuffix && !_hostingEnvironment.IsProduction())
                toSend.Subject = $"{subject} ({_hostingEnvironment.EnvironmentName})";
            else
                toSend.Subject = subject;

            //Add CC's if needed
            if (cc != null)
                foreach (var item in cc)
                    try
                    {
                        toSend.Cc.Add(MailboxAddress.Parse(item));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"Unable to add {item} to email copy list");
                    }

            //Perform templating
            if (_serviceOptions.AlwaysTemplateEmails && string.IsNullOrEmpty(templateName))
                bodyHtml = _emailTemplateFactory.BuildEmailContent(toSend.Subject, bodyHtml);
            else if (!string.IsNullOrEmpty(templateName))
                bodyHtml = _emailTemplateFactory.BuildEmailContent(toSend.Subject, bodyHtml,
                    templateName: templateName);

            var bodyBuilder = new BodyBuilder { HtmlBody = bodyHtml };
            bodyBuilder.Attachments.Add(fileName, fileContent);
            toSend.Body = bodyBuilder.ToMessageBody();
            return toSend;
        }
    }
}