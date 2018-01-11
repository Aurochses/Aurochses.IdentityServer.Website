using Aurochses.IdentityServer.WebSite.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Aurochses.IdentityServer.WebSite.Tests.Helpers
{
    public class ControllerHelpersTests
    {
        private readonly Mock<Controller> _mockController;

        public ControllerHelpersTests()
        {
            _mockController = new Mock<Controller>(MockBehavior.Strict);
            _mockController.Object.ControllerContext = new Mock<ControllerContext>(MockBehavior.Strict).Object;
        }

        [Fact]
        public void AddErrors_Success()
        {
            // Arrange
            var identityError = new IdentityError
            {
                Code = "IdentityErrorCode",
                Description = "IdentityErrorDescription"
            };

            // Act
            _mockController.Object.AddErrors(IdentityResult.Failed(identityError));

            // Assert
            var modelStateItem = Assert.Single(_mockController.Object.ModelState);
            Assert.Equal(string.Empty, modelStateItem.Key);

            var error = Assert.Single(modelStateItem.Value.Errors);
            Assert.NotNull(error);
            Assert.Equal(identityError.Description, error.ErrorMessage);
        }
    }
}