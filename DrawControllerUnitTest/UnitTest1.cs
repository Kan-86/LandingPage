using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DrawControllerUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            var controller = new UserProfilesController(context: null, userService: null);

            // Act
            var result = controller.Index(searchString: null, userLastNames: null);

            // Assert
            var redirectToActionResult =
                Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
