using System.Diagnostics.CodeAnalysis;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ICG.NetCore.Utilities.Email.Smtp
{
    /// <summary>
    /// Implementation of MimeKit for outbound emails
    /// </summary>
    public interface IMimeKitService
    {
        /// <summary>
        /// Sends an email message
        /// </summary>
        /// <param name="toSend">The message to send</param>
        void SendEmail(MimeMessage toSend);
    }

    /// <summary>
    /// This is a low-level service provider that actually processes outbound mail messages
    /// </summary>
    /// <remarks>
    /// Keeping this to ONLY sending emails, we can use upstream processes for data validation and processing.
    /// </remarks>
    [ExcludeFromCodeCoverage] //Excluded to avoid sending actual mail, should be integration tested
    public class MimeKitService : IMimeKitService
    {
        private readonly IOptions<SmtpServiceOptions> _configuration;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Current configuration context</param>
        public MimeKitService(IOptions<SmtpServiceOptions> configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public void SendEmail(MimeMessage toSend)
        {
            using var client = new SmtpClient();
            client.Connect(_configuration.Value.Server, _configuration.Value.Port, _configuration.Value.UseSsl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_configuration.Value.SenderUsername, _configuration.Value.SenderPassword);
            client.Send(toSend);
        }
    }
}