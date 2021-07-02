using AcmeCorporation.Areas.Identity.Data;
using AcmeCorporation.Core.DomainServices;
using AcmeCorporation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeCorporation.Core.ApplicationServices.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userRepo;
        private string message = "";
        public UserProfileService(IUserProfileRepository userRepository)
        {
            _userRepo = userRepository;
        }

        public async Task<string> Draw(string SN, string Email)
        {
            var user = await _userRepo.GetUserByEmail(Email);

            if (user == null)
            {
                message = "User could not be found.";
                return message;
            }
            // Confirm users age
            var age = ConfirmUserAge(user);

            // Make sure Guid is above the 32 digits otherwise return with an error
            if (SN.Length < 32)
            {
                message = "Something went wrong with processing the Serial Number, please try again.";
                return message;
            }

            //var sn = await _userRepo.ConfirmSerialNumber(SN, user.Id);
            var sn = await _userRepo.ConfirmSerialNumber(user);

            if (sn == null)
            {
                message = "Serial number is not valid.";
                return message;
            }

            // Check how many times user has participated, if above the limit we return a error message
            var participationNumber = CheckAmountOfParticipation(user);

            if (participationNumber > 2)
            {
                message = "User is not able to enter the draw more than twice.";
            }
            else if(age >= 18)
            {
                sn.ProductSerialNumber.ValidForLottery = false;
                var usrInDraw = new UserInDraw();
                usrInDraw.HasEnteredAmount = participationNumber;
                usrInDraw.UsersInDraw = new List<UserProfile>();
                usrInDraw.UsersInDraw.Add(user);

                _userRepo.AddToDraw(usrInDraw);

                await _userRepo.InvalidateSerialNumber(sn);

                message = $"{user.FirstName} has successfully entered the draw.";
            }

            return message;
        }

        private int CheckAmountOfParticipation(UserProfile user)
        {
            int timesEntered = 0;
            if (user.UserInDraw != null)
            {
                timesEntered = user.UserInDraw.HasEnteredAmount;
            }
            if (timesEntered <= 2)
            {
                timesEntered += 1;
            }

            return timesEntered;
        }

        private int ConfirmUserAge(UserProfile user)
        {
            var dateNow = DateTime.Today;
            if (user == null)
            {
                message = "Invalid user, try again.";
            }
            var age = dateNow.Year - user.BirthDate.Year;

            if (age < 18)
            {
                message = "Not old enough.  User needs to be 18 or older.";
            }

            return age;
        }

        public bool UserExists(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("Id must be 1 or above.");
            }
            var userExists = _userRepo.UserExists(id);
            return userExists;
        }

        public UserProductsViewModel GetAllUserProfiles(string searchString)
        {
            return _userRepo.GetAllUserProfiles(searchString);
        }
    }
}
