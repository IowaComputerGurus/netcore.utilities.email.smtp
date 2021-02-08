using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;
using Xunit;

namespace ICG.NetCore.Utilities.Email.Smtp.Tests
{
    public class SmtpServiceTests
    {
        private readonly Mock<IMimeKitService> _mimeKitServiceMock;
        private readonly Mock<IMimeMessageFactory> _mimeMessageFactoryMock;
        private readonly SmtpServiceOptions _options = new SmtpServiceOptions()
        {
            AdminEmail = "admin@test.com",
            Port = 15,
            UseSsl = true,
            SenderUsername = "User",
            SenderPassword = "Password",
            Server = "Server",
            AddEnvironmentSuffix = false,
            UseDefaultTemplate = false
        };
        private readonly ISmtpService _service;

        public SmtpServiceTests()
        {
            _mimeKitServiceMock = new Mock<IMimeKitService>();
            _mimeMessageFactoryMock = new Mock<IMimeMessageFactory>();
            _service = new SmtpService(new OptionsWrapper<SmtpServiceOptions>(_options), _mimeMessageFactoryMock.Object,
                _mimeKitServiceMock.Object);
        }

        [Fact]
        public void AdminEmail_ShouldReturnConfigurationEmail()
        {
            //Arrange
            var expectedEmail = "admin@test.com";

            //Act
            var result = _service.AdminEmail;

            //Assert
            Assert.Equal(expectedEmail, result);
        }

        [Fact]
        public void SendToAdministrator_ShouldSend_DefaultingFromAndToAddress()
        {
            //Arrange
            var subject = "Test";
            var message = "Message";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminEmail, null, subject, message))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessageToAdministrator(subject, message);

            //Verify
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }

        [Fact]
        public void SendToAdministrator_ShouldSend_DefaultingFromAndToAddress_WithCCRecipients()
        {
            //Arrange
            var subject = "Test";
            var message = "Message";
            var cc = new List<string> {"recipient@test.com"};
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminEmail, cc, subject, message))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessageToAdministrator(cc, subject, message);

            //Verify
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }

        [Fact]
        public void SendMessage_WithoutCCRecipients_ShouldSend_DefaultingFromAddress()
        {
            //Arrange
            var to = "tester@test.com";
            var subject = "test";
            var message = "message";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, to, null, subject, message))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessage(to, subject, message);

            //Verify
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }

        [Fact]
        public void SendMessage_WithCCRecipients_ShouldSend_DefaultingFromAddress()
        {
            //Arrange
            var to = "tester@test.com";
            var cc = new List<string> {"Person1@test.com"};
            var subject = "test";
            var message = "message";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, to, cc, subject, message))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessage(to, cc, subject, message);

            //Verify
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }

        [Fact]
        public void SendMessageWithAttachment_ShouldSend_DefaultingFromAddress()
        {
            //Arrange
            var to = "tester@test.com";
            var cc = new List<string> { "Person1@test.com" };
            var subject = "test";
            var fileContent = Encoding.ASCII.GetBytes("Testing");
            var fileName = "test.txt";
            var message = "message";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessageWithAttachment(_options.AdminEmail, to, cc, subject, fileContent, fileName, message))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessageWithAttachment(to, cc, subject, fileContent, fileName, message);

            //Assets
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }
    }
}
