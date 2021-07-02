using AcmeCorporation.Areas.Identity.Data;
using AcmeCorporation.Controllers;
using AcmeCorporation.Core.ApplicationServices;
using AcmeCorporation.Core.ApplicationServices.Services;
using AcmeCorporation.Core.DomainServices;
using AcmeCorporation.Data;
using AcmeCorporation.Infastructure;
using AcmeCorporation.Infastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserProfileTest
{
    [TestClass]
    public class UserProfileDrawTests
    {

        SqliteConnection connection;
        UserProfileService profileService;

        public UserProfileDrawTests()
        {
            connection = new SqliteConnection("DataSource=:memory:");

            // In-memory database only exists while the connection is open
            connection.Open();

            // Initialize test database
            var options = new DbContextOptionsBuilder<DrawContext>()
                            .UseSqlite(connection).Options;
            var dbContext = new DrawContext(options);
            IDbInitializer dbInitializer = new DbInitializer();
            dbInitializer.Initialize(dbContext);

            // Create repositories and BookingManager
            var userProfileRepos = new UserProfileRepository(dbContext);
            profileService = new UserProfileService(userProfileRepos);
        }

        public void Dispose()
        {
            // This will delete the in-memory database
            connection.Close();
        }

        /*
            Test to see if user exists
        */
        [TestMethod]
        public void TestIfUserExists()
        {
            // Act
            var userId = profileService.UserExists(1);
            // Assert
            Assert.AreEqual(true, userId);
        }

        /*
            Test to see if we get an exception if the id is lower than 1
         */
        [TestMethod]
        public void TestIfUserExists_IdIsLowerThanOne()
        {
            var id = 0;

            Exception ex = Assert.ThrowsException<ArgumentException>(() =>
            profileService.UserExists(id));
            Assert.AreEqual("Id must be 1 or above.", ex.Message);
        }

        [TestMethod]
        public void TestIfUserCannotBeFound()
        {
            string SN = "", Email = "";

            var findUser = profileService.Draw(SN, Email);
            Assert.AreEqual(findUser.Result, "User could not be found.");
        }

        [TestMethod]
        public void TestIfUserCanEnterTwice()
        {
            string SN = "29186B89-9B20-40A4-8EF5-13D2A114A9A8", Email = "testa@testa.com";
            Guid.Parse(SN);
            var findUser = profileService.Draw(SN, Email);
            Assert.AreEqual(findUser.Result, "User is not able to enter the draw more than twice.");
        }

        [TestMethod]
        public void TestIfUserIsOldEnough()
        {
            string SN = "55D658AD-338E-4885-8626-D0872918FA42", Email = "tester@tester.com";
            Guid.Parse(SN);
            var findUser = profileService.Draw(SN, Email);
            Assert.AreEqual(findUser.Result, "Not old enough.  User needs to be 18 or older.");
        }

        [TestMethod]
        public void TestIfUserSerialNumberIsValidGuid()
        {
            string SN = "", Email = "test@test.com";

            var findUser = profileService.Draw(SN, Email);
            Assert.AreEqual(findUser.Result, "Something went wrong with processing the Serial Number, please try again.");
        }
    }
}
