﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ICG.NetCore.Utilities.Email.Smtp.Tests
{
    public class MimeMessageFactoryTests
    {
        private readonly Mock<ILogger<MimeMessageFactory>> _loggerMock;
        private readonly IMimeMessageFactory _factory;
        private readonly Mock<IEmailTemplateFactory> _emailTemplateFactoryMock;
        private readonly Mock<IHostEnvironment> _hostingEnvironment;
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

        public MimeMessageFactoryTests()
        {
            _loggerMock = new Mock<ILogger<MimeMessageFactory>>();
            _hostingEnvironment = new Mock<IHostEnvironment>();
            _emailTemplateFactoryMock = new Mock<IEmailTemplateFactory>();
            _factory = new MimeMessageFactory(new OptionsWrapper<SmtpServiceOptions>(_options), _loggerMock.Object, _hostingEnvironment.Object, _emailTemplateFactoryMock.Object);
        }


        [Fact]
        public void CreateFromMessageShouldReturnANonNullResultWithAllInputsProvided()
        {
            //Arrange
            var from = "Test@test.com";
            var to = "test@test.com";
            var bodyHtml = "<p></p>";
            var subject = "Test";

            //Act
            var result = _factory.CreateFromMessage(from,string.Empty, to, subject, bodyHtml);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CreateFromMessageShouldReturnSingleFromAddressResultWithValidInputs()
        {
            //Arrange
            var from = "Test@test.com";
            var to = "test@test.com";
            var bodyHtml = "<p></p>";
            var subject = "Test";

            //Act
            var result = _factory.CreateFromMessage(from, string.Empty, to, subject, bodyHtml);

            //Assert
            Assert.NotNull(result.From);
            Assert.Single(result.From);
        }

        [Fact]
        public void CreateFromMessageShouldReturnProperFromAddressWithValidInputs()
        {
            //Arrange
            var from = "Test@test.com";
            var to = "test@test.com";
            var bodyHtml = "<p></p>";
            var subject = "Test";

            //Act
            var result = _factory.CreateFromMessage(from, string.Empty, to, subject, bodyHtml);

            //Assert
            Assert.Equal(from, result.From[0].ToString());
        }

        [Fact]
        public void CreateFromMessageShouldReturnSingleToAddressResultWithValidInputs()
        {
            //Arrange
            var from = "Test@test.com";
            var to = "test@test.com";
            var bodyHtml = "<p></p>";
            var subject = "Test";

            //Act
            var result = _factory.CreateFromMessage(from, string.Empty, to, subject, bodyHtml);

            //Assert
            Assert.NotNull(result.To);
            Assert.Single(result.To);
        }

        [Fact]
        public void CreateFromMessageShouldReturnProperToAddressWithValidInputs()
        {
            //Arrange
            var from = "Test@test.com";
            var to = "test@test.com";
            var bodyHtml = "<p></p>";
            var subject = "Test";

            //Act
            var result = _factory.CreateFromMessage(from,string.Empty, to, subject, bodyHtml);

            //Assert
            Assert.Equal(to, result.To[0].ToString());
        }

        [Fact]
        public void CreateFromMessageShouldReturnProperCCAddressWithValidInputs()
        {
            //Arrange
            var from = "Test@test.com";
            var to = "test@test.com";
            var bodyHtml = "<p></p>";
            var subject = "Test";
            var cc = new List<string> { "testing@tester.com" };

            //Act
            var result = _factory.CreateFromMessage(from, string.Empty, to, cc, subject, bodyHtml);

            //Assert
            Assert.Equal(cc[0], result.Cc[0].ToString());
        }


        [Fact]
        public void CreateFromMessageShouldReturnProperCCAddressWithValidInputs_SkippingInvalidEntries()
        {
            //Arrange
            var from = "Test@test.com";
            var to = "test@test.com";
            var bodyHtml = "<p></p>";
            var subject = "Test";
            var cc = new List<string> { "testing@tester.com", " " };

            //Act
            var result = _factory.CreateFromMessage(from, string.Empty, to, cc, subject, bodyHtml);

            //Assert
            Assert.Single(result.Cc);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Unable to add   to email copy list", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }


        [Fact]
        public void CreateFromMessageShouldThrowArgumentExceptionWhenFromIsEmpty()
        {
            //Arrange
            var from = string.Empty;
            var to = string.Empty;
            var subject = string.Empty;
            var bodyHtml = string.Empty;

            //Act/Assert
            Assert.Throws<ArgumentNullException>("from", () => _factory.CreateFromMessage(from, string.Empty, to, subject, bodyHtml));
        }

        [Fact]
        public void CreateFromMessageShouldThrowArgumentExceptionWhenToIsEmpty()
        {
            //Arrange
            var from = "Test@test.com";
            var to = string.Empty;
            var subject = string.Empty;
            var bodyHtml = string.Empty;

            //Act/Assert
            Assert.Throws<ArgumentNullException>("to", () => _factory.CreateFromMessage(from, string.Empty, to, subject, bodyHtml));
        }

        [Fact]
        public void CreateFromMessageShouldThrowArgumentExceptionWhenSubjectIsEmpty()
        {
            //Arrange
            var from = "Test@test.com";
            var to = "test@test.com";
            var subject = string.Empty;
            var bodyHtml = string.Empty;

            //Act/Assert
            Assert.Throws<ArgumentNullException>("subject",
                () => _factory.CreateFromMessage(from, string.Empty, to, subject, bodyHtml));
        }

        [Fact]
        public void CreateFromMessageShouldThrowArgumentExceptionWhenBodyHtmlIsEmpty()
        {
            //Arrange
            var from = "Test@test.com";
            var to = "test@test.com";
            var subject = "testing";
            var bodyHtml = string.Empty;

            //Act/Assert
            Assert.Throws<ArgumentNullException>("bodyHtml",
                () => _factory.CreateFromMessage(from, string.Empty, to, subject, bodyHtml));
        }

        #region CreateFromMessageWithAttachment

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldThrowArgumentNull_WhenMissingFromAddress()
        {
            //arrange

            //Act
            Assert.Throws<ArgumentNullException>("fromAddress",
                () => _factory.CreateFromMessageWithAttachment(null, string.Empty, null, null, null, null,
                    null, null));
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldThrowArgumentNull_WhenMissingToAddress()
        {
            //arrange
            var from = "test@test.com";

            //Act
            Assert.Throws<ArgumentNullException>("toAddress",
                () => _factory.CreateFromMessageWithAttachment(from, string.Empty, null, null, null, null,
                    null, null));
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldThrowArgumentNull_WhenMissingSubject()
        {
            //arrange
            var from = "test@test.com";
            var to = "recipient@test.com";

            //Act
            Assert.Throws<ArgumentNullException>("subject",
                () => _factory.CreateFromMessageWithAttachment(from, string.Empty, to, null, null, null,
                    null, null));
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldThrowArgumentNull_WhenMissingFileContent()
        {
            //arrange
            var from = "test@test.com";
            var to = "recipient@test.com";
            var subject = "My Message";

            //Act
            Assert.Throws<ArgumentNullException>("fileContent",
                () => _factory.CreateFromMessageWithAttachment(from, string.Empty, to, null, subject, null,
                    null, null));
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldThrowArgumentNull_WhenMissingFileName()
        {
            //arrange
            var from = "test@test.com";
            var to = "recipient@test.com";
            var subject = "My Message";
            var fileContent = Encoding.ASCII.GetBytes("Testing");

            //Act
            Assert.Throws<ArgumentNullException>("fileName",
                () => _factory.CreateFromMessageWithAttachment(from, string.Empty, to, null, subject, fileContent,
                    null, null));
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldThrowArgumentNull_WhenMissingMessageBody()
        {
            //arrange
            var from = "test@test.com";
            var to = "recipient@test.com";
            var subject = "My Message";
            var fileContent = Encoding.ASCII.GetBytes("Testing");
            var fileName = "text.txt";

            //Act
            Assert.Throws<ArgumentNullException>("bodyHtml",
                () => _factory.CreateFromMessageWithAttachment(from, string.Empty, to, null, subject, fileContent,
                    fileName, null));
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ReturnValidMessage_WithProperInputs()
        {
            //arrange
            var from = "test@test.com";
            var to = "recipient@test.com";
            var subject = "My Message";
            var fileContent = Encoding.ASCII.GetBytes("Testing");
            var fileName = "text.txt";
            var bodyHtml = "<p>Hello!</p>";

            //Act
            var result =
                _factory.CreateFromMessageWithAttachment(from, string.Empty, to, null, subject, fileContent, fileName, bodyHtml);

            //Assert
            Assert.Equal(from, result.From[0].ToString());
            Assert.Equal(to, result.To[0].ToString());
            Assert.Equal(subject, result.Subject);
            Assert.Equal(bodyHtml, result.HtmlBody);
            Assert.NotNull(result.Attachments);
            Assert.Single(result.Attachments);
        }


        [Fact]
        public void CreateFromMessageWithAttachment_ReturnValidMessage_WithCCAndInput()
        {
            //arrange
            var from = "test@test.com";
            var to = "recipient@test.com";
            var subject = "My Message";
            var fileContent = Encoding.ASCII.GetBytes("Testing");
            var fileName = "text.txt";
            var bodyHtml = "<p>Hello!</p>";
            var cc = new List<string> { "copies@you.com" };

            //Act
            var result =
                _factory.CreateFromMessageWithAttachment(from, string.Empty, to, cc, subject, fileContent, fileName, bodyHtml);

            //Assert
            Assert.Equal(cc[0], result.Cc[0].ToString());
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ReturnValidMessage_WithCCAndInput_SkippingInvalidEntries()
        {
            //arrange
            var from = "test@test.com";
            var to = "recipient@test.com";
            var subject = "My Message";
            var fileContent = Encoding.ASCII.GetBytes("Testing");
            var fileName = "text.txt";
            var bodyHtml = "<p>Hello!</p>";
            var cc = new List<string> { "copies@you.com", " " };

            //Act
            var result =
                _factory.CreateFromMessageWithAttachment(from, string.Empty, to, cc, subject, fileContent, fileName, bodyHtml);

            //Assert
            Assert.Single(result.Cc);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals("Unable to add   to email copy list", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
        #endregion

        #region Settings Based Custom Subject

        [Fact]
        public void CreateFromMessage_ShouldModifyUrl_WhenSuffixEnabledAndNotProduction()
        {
            //Arrange
            var options = new SmtpServiceOptions()
            {
                AddEnvironmentSuffix = true,
                AlwaysTemplateEmails = false
            };
            var factory = new MimeMessageFactory(new OptionsWrapper<SmtpServiceOptions>(options), _loggerMock.Object, _hostingEnvironment.Object, _emailTemplateFactoryMock.Object );
            _hostingEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
            var from = "from@test.com";
            var to = "to@test.com";
            var subject = "Subject";
            var bodyHtml = "<p>Test</p>";
            var expectedSubject = "Subject (Development)";

            //Act
            var result = factory.CreateFromMessage(from, string.Empty, to, null, subject, bodyHtml);

            //Assert
            Assert.Equal(expectedSubject, result.Subject);
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldModifyUrl_WhenSuffixEnabledAndNotProduction()
        {
            //Arrange
            var options = new SmtpServiceOptions()
            {
                AddEnvironmentSuffix = true,
                AlwaysTemplateEmails = false
            };
            var factory = new MimeMessageFactory(new OptionsWrapper<SmtpServiceOptions>(options), _loggerMock.Object, _hostingEnvironment.Object, _emailTemplateFactoryMock.Object);
            _hostingEnvironment.Setup(e => e.EnvironmentName).Returns("Development");
            var from = "from@test.com";
            var to = "to@test.com";
            var fileContent = Encoding.ASCII.GetBytes("Testing");
            var fileName = "test.txt";
            var subject = "Subject";
            var bodyHtml = "<p>Test</p>";
            var expectedSubject = "Subject (Development)";

            //Act
            var result = factory.CreateFromMessageWithAttachment(from, string.Empty, to, null, subject, fileContent, fileName, bodyHtml);

            //Assert
            Assert.Equal(expectedSubject, result.Subject);
        }
        #endregion

        #region Settings Based Templating

        [Fact]
        public void CreateFromMessage_ShouldSendToTemplateFactory_WhenAlwaysUseTemplatedEmail()
        {
            //Arrange
            var options = new SmtpServiceOptions
            {
                AlwaysTemplateEmails = true
            };
            var factory = new MimeMessageFactory(new OptionsWrapper<SmtpServiceOptions>(options), _loggerMock.Object,
                _hostingEnvironment.Object, _emailTemplateFactoryMock.Object);
            var from = "from@test.com";
            var to = "to@test.com";
            var subject = "Subject";
            var bodyHtml = "<p>Test</p>";
            var updatedHtml = "<h1>Templated</h1><p>Test</p>";
            _emailTemplateFactoryMock
                .Setup(e => e.BuildEmailContent(subject, bodyHtml, "", ""))
                .Returns(updatedHtml)
                .Verifiable();

            //Act
            var result = factory.CreateFromMessage(from, string.Empty, to, null, subject, bodyHtml);

            //Assert
            _emailTemplateFactoryMock.Verify();
            Assert.Equal(updatedHtml, result.HtmlBody);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CreateFromMessage_ShouldSendToTemplateFactory_WithCustomTemplate(bool alwaysTemplateSetting)
        {
            //Arrange
            var options = new SmtpServiceOptions
            {
                AlwaysTemplateEmails = alwaysTemplateSetting
            };
            var factory = new MimeMessageFactory(new OptionsWrapper<SmtpServiceOptions>(options), _loggerMock.Object,
                _hostingEnvironment.Object, _emailTemplateFactoryMock.Object);
            var from = "from@test.com";
            var to = "to@test.com";
            var subject = "Subject";
            var bodyHtml = "<p>Test</p>";
            var requestedTemplate = "testing";
            var updatedHtml = "<h1>Templated</h1><p>Test</p>";
            _emailTemplateFactoryMock
                .Setup(e => e.BuildEmailContent(subject, bodyHtml, "", requestedTemplate))
                .Returns(updatedHtml)
                .Verifiable();

            //Act
            var result = factory.CreateFromMessage(from, string.Empty, to, null, subject, bodyHtml, requestedTemplate);

            //Assert
            _emailTemplateFactoryMock.Verify();
            Assert.Equal(updatedHtml, result.HtmlBody);
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldSendToTemplateFactory_WhenAlwaysUseTemplatedEmail()
        {
            //Arrange
            var options = new SmtpServiceOptions
            {
                AlwaysTemplateEmails = true
            };
            var factory = new MimeMessageFactory(new OptionsWrapper<SmtpServiceOptions>(options), _loggerMock.Object,
                _hostingEnvironment.Object, _emailTemplateFactoryMock.Object);
            var from = "from@test.com";
            var to = "to@test.com";
            var subject = "Subject";
            var attachment = Encoding.ASCII.GetBytes("Testing");
            var fileName = "file.txt";
            var bodyHtml = "<p>Test</p>";
            var updatedHtml = "<h1>Templated</h1><p>Test</p>";
            _emailTemplateFactoryMock
                .Setup(e => e.BuildEmailContent(subject, bodyHtml, "", ""))
                .Returns(updatedHtml)
                .Verifiable();

            //Act
            var result = factory.CreateFromMessageWithAttachment(from, string.Empty, to, null, subject, attachment, fileName, bodyHtml);

            //Assert
            _emailTemplateFactoryMock.Verify();
            Assert.Equal(updatedHtml, result.HtmlBody);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CreateFromMessageWithAttachment_ShouldSendToTemplateFactory_WithCustomTemplate(bool alwaysTemplateSetting)
        {
            //Arrange
            var options = new SmtpServiceOptions
            {
                AlwaysTemplateEmails = alwaysTemplateSetting
            };
            var factory = new MimeMessageFactory(new OptionsWrapper<SmtpServiceOptions>(options), _loggerMock.Object,
                _hostingEnvironment.Object, _emailTemplateFactoryMock.Object);
            var from = "from@test.com";
            var to = "to@test.com";
            var attachment = Encoding.ASCII.GetBytes("Testing");
            var fileName = "file.txt";
            var subject = "Subject";
            var bodyHtml = "<p>Test</p>";
            var requestedTemplate = "testing";
            var updatedHtml = "<h1>Templated</h1><p>Test</p>";
            _emailTemplateFactoryMock
                .Setup(e => e.BuildEmailContent(subject, bodyHtml, "", requestedTemplate))
                .Returns(updatedHtml)
                .Verifiable();

            //Act
            var result = factory.CreateFromMessageWithAttachment(from, string.Empty, to, null, subject, attachment, fileName, bodyHtml, requestedTemplate);

            //Assert
            _emailTemplateFactoryMock.Verify();
            Assert.Equal(updatedHtml, result.HtmlBody);
        }
        #endregion
    }
}