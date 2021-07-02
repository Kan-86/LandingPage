using AcmeCorporation.Areas.Identity.Data;
using AcmeCorporation.Core.DomainServices;
using AcmeCorporation.Data;
using AcmeCorporation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeCorporation.Infastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly DrawContext _context;
        public UserProfileRepository(DrawContext context)
        {
            _context = context;
        }
        public  void AddToDraw(UserInDraw usrInDraw)
        {
            int id = 0;
            foreach (var item in usrInDraw.UsersInDraw)
            {
                id = item.Id;
            }

            // 
            var local = _context.Set<UserInDraw>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(id));

            // check if local is not null 
            if (local != null)
            {
                // detach
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.UserInDraw.Attach(usrInDraw).State = EntityState.Added;

            _context.SaveChanges();

        }

        public UserProfile ConfirmUser(int id)
        {
            var usr = _context.UserProfile.Where(u => u.Id == id).FirstOrDefault();
            return usr;
        }

        public async Task<Product> ConfirmSerialNumber(UserProfile user)
        {
            var guid = new Guid();

            var findSerialNumber = await _context.UserProfile
                .Where(id => id.Id == user.Id)
                .Include(prod => prod.UserProducts)
                .ThenInclude(sn => sn.ProductSerialNumber)
                .SelectMany(sn => sn.UserProducts)
                .FirstOrDefaultAsync();

            guid = findSerialNumber.ProductSerialNumber.ProductSerialNumber;

            var product = await _context.Product
                .Where(s => s.ProductSerialNumber.ProductSerialNumber == guid
                            && s.ProductSerialNumber.ValidForLottery == true)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return null;
            }
            var confirmSn = await _context.UserProfile
                .Include(p => p.UserProducts)
                .ThenInclude(sn => sn.ProductSerialNumber)
                .SelectMany(pr => pr.UserProducts)
                .Where(p => p.Id == product.Id)
                .FirstOrDefaultAsync();

            //var findSerialNumber = await _context.UserProfile
            //    .Where(id => id.Id == user.Id)
            //    .Include(p => p.UserProducts)
            //    .ThenInclude(r => r.ProductSerialNumber)
            //    .SelectMany(p => p.UserProducts)
            //    .Where(r => r.ProductSerialNumber.ProductSerialNumber == guid
            //               && r.ProductSerialNumber.ValidForLottery == true)
            //    .FirstOrDefaultAsync();

            if (confirmSn == null)
            {
                return null;
            }

            return confirmSn;
        }

        public async Task<UserProfile> GetUserByEmail(string email)
        {
            var user = await _context.Users
                .Where(e => e.Email == email)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return null;
            }
            var userProfile = await _context.UserProfile
                .Include(prod => prod.UserProducts)
                .Include(sn => sn.UserInDraw)
                .Where(i => i.Id == user.UserProfileId)
                .FirstOrDefaultAsync();

            return userProfile;
        }

        public bool UserExists(int id)
        {
            return _context.UserProfile.Any(e => e.Id == id);
        }

        public async Task InvalidateSerialNumber(Product sn)
        {
            var prodSn = await _context.Product
                .Include(sn => sn.ProductSerialNumber)
                .Where(i => i.ProductSerialNumberId == sn.ProductSerialNumberId)
                .FirstOrDefaultAsync();

            _context.Product.Attach(prodSn).State = EntityState.Modified;
        }

        public UserProductsViewModel GetAllUserProfiles(string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<string> lastNameQuery = from m in _context.UserProfile
                                               orderby m.LastName
                                               select m.LastName;

            var users = from m in _context.UserProfile
                        .Include(s => s.UserProducts)
                        select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
            }

            var userQueries = new UserProductsViewModel
            {
                LastName = new SelectList(lastNameQuery.Distinct().ToList()),
                Users = users.ToList()
            };
            return userQueries;
        }
    }
}
    