using AcmeCorporation.Areas.Identity.Data;
using AcmeCorporation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeCorporation.Core.ApplicationServices
{
    public interface IUserProfileService
    {
        Task<string> Draw(string SN, string Email);
        bool UserExists(int id);
        UserProductsViewModel GetAllUserProfiles(string searchString);
    }
}
