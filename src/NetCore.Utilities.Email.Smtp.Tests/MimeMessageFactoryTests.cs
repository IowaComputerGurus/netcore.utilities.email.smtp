using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ICG.NetCore.Utilities.Email.Smtp.Tests
{
    public class MimeMessageFactoryTests
    {
        private readonly Mock<ILogger> _loggerMock;
        private readonly IMimeMessageFactory _factory;

        public MimeMessageFactoryTests()
        {
            _loggerMock = new Mock<ILogger>();
            _factory = new MimeMessageFactory(_loggerMock.Object);
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
            var result = _factory.CreateFromMessage(from, to, subject, bodyHtml);

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
            var result = _factory.CreateFromMessage(from, to, subject, bodyHtml);

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
            var result = _factory.CreateFromMessage(from, to, subject, bodyHtml);

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
            var result = _factory.CreateFromMessage(from, to, subject, bodyHtml);

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
            var result = _factory.CreateFromMessage(from, to, subject, bodyHtml);

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
            var cc = new List<string> {"testing@tester.com"};

            //Act
            var result = _factory.CreateFromMessage(from, to, cc, subject, bodyHtml);

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
            var result = _factory.CreateFromMessage(from, to, cc, subject, bodyHtml);

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
            Assert.Throws<ArgumentNullException>("from", () => _factory.CreateFromMessage(from, to, subject, bodyHtml));
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
            Assert.Throws<ArgumentNullException>("to", () => _factory.CreateFromMessage(from, to, subject, bodyHtml));
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
                () => _factory.CreateFromMessage(from, to, subject, bodyHtml));
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
                () => _factory.CreateFromMessage(from, to, subject, bodyHtml));
        }

        #region CreateFromMessageWithAttachment

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldThrowArgumentNull_WhenMissingFromAddress()
        {
            //arrange

            //Act
            Assert.Throws<ArgumentNullException>("fromAddress",
                () => _factory.CreateFromMessageWithAttachment(null, null, null, null, null,
                    null, null));
        }

        [Fact]
        public void CreateFromMessageWithAttachment_ShouldThrowArgumentNull_WhenMissingToAddress()
        {
            //arrange
            var from = "test@test.com";

            //Act
            Assert.Throws<ArgumentNullException>("toAddress",
                () => _factory.CreateFromMessageWithAttachment(from, null, null, null, null,
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
                () => _factory.CreateFromMessageWithAttachment(from, to, null, null, null,
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
                () => _factory.CreateFromMessageWithAttachment(from, to, null, subject, null,
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
                () => _factory.CreateFromMessageWithAttachment(from, to, null, subject, fileContent,
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
                () => _factory.CreateFromMessageWithAttachment(from, to, null, subject, fileContent,
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
                _factory.CreateFromMessageWithAttachment(from, to, null, subject, fileContent, fileName, bodyHtml);

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
            var cc = new List<string> {"copies@you.com"};

            //Act
            var result =
                _factory.CreateFromMessageWithAttachment(from, to, cc, subject, fileContent, fileName, bodyHtml);

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
                _factory.CreateFromMessageWithAttachment(from, to, cc, subject, fileContent, fileName, bodyHtml);

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
    }
}