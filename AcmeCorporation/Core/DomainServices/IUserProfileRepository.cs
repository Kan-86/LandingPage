using AcmeCorporation.Areas.Identity.Data;
using AcmeCorporation.Models;
using System;
using System.Threading.Tasks;

namespace AcmeCorporation.Core.DomainServices
{
    public interface IUserProfileRepository
    {
        bool UserExists(int id);
        void AddToDraw(UserInDraw usrInDraw);
        UserProfile ConfirmUser(int id);
        Task<UserProfile> GetUserByEmail(string email);
        //Task<Product> ConfirmSerialNumber(string sN, int id);
        Task<Product> ConfirmSerialNumber(UserProfile user);
        Task InvalidateSerialNumber(Product sn);
        UserProductsViewModel GetAllUserProfiles(string searchString);
    }
}
