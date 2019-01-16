using System;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ICG.NetCore.Utilities.Email.Tests
{
    public class MimeMessageFactoryTests
    {
        public MimeMessageFactoryTests()
        {
            _factory = new MimeMessageFactory(_iLoggerFactoryMock.Object);
        }

        private readonly Mock<ILoggerFactory> _iLoggerFactoryMock = new Mock<ILoggerFactory>();
        private readonly IMimeMessageFactory _factory;

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
    }
}