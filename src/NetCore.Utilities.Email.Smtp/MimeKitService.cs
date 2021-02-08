using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ICG.NetCore.Utilities.Email.Smtp
{
    /// <summary>
    /// Service implementation to send emails using MimeKit
    /// </summary>
    public interface IMimeKitService
    {
        /// <summary>
        /// Sends an email message
        /// </summary>
        /// <param name="toSend"></param>
        void SendEmail(MimeMessage toSend);
    }

    /// <inheritdoc />
    public class MimeKitService : IMimeKitService
    {
        private readonly SmtpServiceOptions _configuration;

        /// <summary>
        /// Default Constructor with DI configuration
        /// </summary>
        /// <param name="configuration">The configuration for the SMTP Service</param>
        public MimeKitService(IOptions<SmtpServiceOptions> configuration)
        {
            _configuration = configuration.Value;
        }

        /// <inheritdoc />
        public void SendEmail(MimeMessage toSend)
        {
            using (var client = new SmtpClient())
            {
                client.Connect(_configuration.Server, _configuration.Port, _configuration.UseSsl);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_configuration.SenderUsername, _configuration.SenderPassword);
                client.Send(toSend);
            }
        }
    }
}