using ICG.NetCore.Utilities.UnitTesting;
using Xunit;

namespace ICG.NetCore.Utilities.Email.Smtp.Tests
{
    public class SmtpServiceOptionsTests : AbstractModelTest
    {
        [Theory]
        [InlineData("AdminEmail", "Admin Email")]
        [InlineData("Server", "Server")]
        [InlineData("Port", "Port")]
        [InlineData("UseSsl", "Use SSL")]
        [InlineData("SenderUsername", "Sender Username")]
        [InlineData("SenderPassword", "Sender Password")]
        public void DisplayPropertiesShouldHaveDisplayNamesDefined(string property, string expectedText)
        {
            //Act/Asset
            AssertDisplayAttribute(typeof(SmtpServiceOptions), property, expectedText);
        }
    }
}