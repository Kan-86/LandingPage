using AcmeCorporation.Controllers;
using AcmeCorporation.Core.ApplicationServices.Services;
using AcmeCorporation.Data;
using AcmeCorporation.Infastructure;
using AcmeCorporation.Infastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DrawControllerTests
{
    public class UserProfileControllerTests
    {

        [Fact]
        public async Task TestTheDrawForm_ToVarifyRedirectionWorks()
        {
            // Arrange
            var controller =  new UserProfilesController(context: null, userService: null);

            // Act
            var result = await controller.DrawForm(SN: null, Email: "test@test.com");

            // Assert
            var redirectToActionResult =
                Assert.IsType<RedirectToActionResult>(result);
            Assert.DoesNotMatch("DrawForm", redirectToActionResult.ControllerName);
            Assert.Matches("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task TestTheDetails_ToVarifyRedirectionWorks()
        {
            // Arrange
            var controller = new UserProfilesController(context: null, userService: null);

            // Act
            var result = await controller.Details(id: null);

            // Assert
            var redirectToActionResult =
                Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
