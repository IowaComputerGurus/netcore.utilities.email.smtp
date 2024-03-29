﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            AlwaysTemplateEmails = false
        };
        private readonly IEmailService _service;

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
        public void AdminEmail_ShouldReturnNullWhenNoConfiguration()
        {
            //Arrange
            var testService = new SmtpService(new OptionsWrapper<SmtpServiceOptions>(null), _mimeMessageFactoryMock.Object, _mimeKitServiceMock.Object);

            //Act
            var result = testService.AdminEmail;

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void SendToAdministrator_ShouldSend_DefaultingFromAndToAddress()
        {
            //Arrange
            var subject = "Test";
            var message = "Message";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminName, _options.AdminEmail, null, subject, message, ""))
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
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminName, _options.AdminEmail, cc, subject, message, ""))
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
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminName, to, null, subject, message, ""))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessage(to, subject, message);

            //Verify
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }

        [Fact]
        public void SendMessageWithReplyTo_WithoutCCRecipients_ShouldSend_DefaultingFromAddress()
        {
            //Arrange
            var replyTo = "me@me.com";
            var replyToName = "Bob";
            var to = "tester@test.com";
            var subject = "test";
            var message = "message";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminName, to, null, subject, message, ""))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendWithReplyTo(replyTo, replyToName, to, subject, message);

            //Verify
            Assert.Equal(1, mimeMessage.ReplyTo.Count);
            var replyToAsAdded = mimeMessage.ReplyTo.First();
            Assert.Equal("\"Bob\" <me@me.com>", replyToAsAdded.ToString());
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
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminName, to, cc, subject, message, ""))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessage(to, cc, subject, message);

            //Verify
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }

        [Fact]
        public void SendMessageWithReplyTo_WithCCRecipients_ShouldSend_DefaultingFromAddress()
        {
            //Arrange
            var replyTo = "me@me.com";
            var replyToName = "Bob";
            var to = "tester@test.com";
            var cc = new List<string> { "Person1@test.com" };
            var subject = "test";
            var message = "message";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminName, to, cc, subject, message, ""))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendWithReplyTo(replyTo, replyToName, to, cc, subject, message);

            //Verify
            Assert.Equal(1, mimeMessage.ReplyTo.Count);
            var replyToAsAdded = mimeMessage.ReplyTo.First();
            Assert.Equal("\"Bob\" <me@me.com>", replyToAsAdded.ToString());
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
                .Setup(f => f.CreateFromMessageWithAttachment(_options.AdminEmail, _options.AdminName, to, cc, subject, fileContent, fileName, message, ""))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessageWithAttachment(to, cc, subject, fileContent, fileName, message, null);

            //Assets
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }

        [Fact]
        public void SendMessage_ShouldPassOptionalTemplateName_ToMessageMethods()
        {
            //Arrange
            var to = "tester@test.com";
            var cc = new List<string> { "Person1@test.com" };
            var subject = "test";
            var message = "message";
            var requestedTemplate = "Test";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminName, to, cc, subject, message, requestedTemplate))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessage(to, cc, subject, message, null, requestedTemplate);

            //Assets
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }

        [Fact]
        public void SendMessageWithReplyTo_ShouldPassOptionalTemplateName_ToMessageMethods()
        {
            //Arrange
            var replyTo = "me@me.com";
            var replyToName = "Bob";
            var to = "tester@test.com";
            var cc = new List<string> { "Person1@test.com" };
            var subject = "test";
            var message = "message";
            var requestedTemplate = "Test";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessage(_options.AdminEmail, _options.AdminName, to, cc, subject, message, requestedTemplate))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendWithReplyTo(replyTo, replyToName, to, cc, subject, message, null, requestedTemplate);

            //Assets
            Assert.Equal(1, mimeMessage.ReplyTo.Count);
            var replyToAsAdded = mimeMessage.ReplyTo.First();
            Assert.Equal("\"Bob\" <me@me.com>", replyToAsAdded.ToString());
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }

        [Fact]
        public void SendMessageWithAttachment_ShouldPassOptionalTemplateName_ToMessageMethods()
        {
            //Arrange
            var to = "tester@test.com";
            var cc = new List<string> { "Person1@test.com" };
            var subject = "test";
            var fileContent = Encoding.ASCII.GetBytes("Testing");
            var fileName = "test.txt";
            var message = "message";
            var requestedTemplate = "Test";
            var mimeMessage = new MimeMessage();
            _mimeMessageFactoryMock
                .Setup(f => f.CreateFromMessageWithAttachment(_options.AdminEmail, _options.AdminName, to, cc, subject, fileContent, fileName, message, requestedTemplate))
                .Returns(mimeMessage).Verifiable();

            //Act
            _service.SendMessageWithAttachment(to, cc, subject, fileContent, fileName, message, null, requestedTemplate);

            //Assets
            _mimeMessageFactoryMock.Verify();
            _mimeKitServiceMock.Verify(k => k.SendEmail(mimeMessage));
        }
    }
}
