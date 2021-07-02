using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcmeCorporation.Areas.Identity.Data;
using AcmeCorporation.Data;
using Microsoft.AspNetCore.Authorization;
using AcmeCorporation.Core.ApplicationServices;
using X.PagedList;

namespace AcmeCorporation.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly DrawContext _context;
        private readonly IUserProfileService _userService;
        public UserProfilesController(DrawContext context, IUserProfileService userService)
        {
            _context = context;
            _userService = userService;
        }
        [Authorize]
        // GET: UserProfiles
        public async  Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            

            var userProf = from s in _context.UserProfile
                           .Include(ud => ud.UserInDraw)
                           select s;

            if (userProf == null)
            {
                return RedirectToAction(actionName: nameof(Index),
                     controllerName: "Home");
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                userProf = userProf.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstName.Contains(searchString));
            }
            var pageNumber = page ?? 1;
            var pageSize = 10; //Show 10 rows every time
            var brands = await userProf.ToPagedListAsync(pageNumber, pageSize);
            return View(brands);
        }

        [Authorize]
        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(actionName: nameof(Index),
                    controllerName: "Home");
            }
            var userProfile = await _context.UserProfile
                .Include(u => u.User)
                .Include(p => p.UserProducts)
                .ThenInclude(sn => sn.ProductSerialNumber)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProfile == null)
            {
                return RedirectToAction(actionName: nameof(Index),
                    controllerName: "Home");
            }

            return View(userProfile);
        }

        
        [Authorize]
        public async Task<IActionResult> DrawForm(string SN, string Email)
        {
            if (!string.IsNullOrEmpty(SN) && !string.IsNullOrEmpty(Email))
            {
                var validation = await _userService.Draw(SN, Email);
                TempData["testmsg"] = validation;
                return RedirectToAction(nameof(Index));
            }
            else if (string.IsNullOrEmpty(SN) && !string.IsNullOrEmpty(Email))
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: UserProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            return View(userProfile);
        }

        public IList<UserProfile> UserProds { get; set; }

        public async Task OnGetAsync()
        {
            UserProds = await _context.UserProfile
                .Include(c => c.UserProducts)
                .AsNoTracking()
                .ToListAsync();
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,BirthDate,UserInDrawId")] UserProfile userProfile)
        {
            if (id != userProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!UserProfileExists(userProfile.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userProfile);
        }

        // GET: UserProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userProfile = await _context.UserProfile.FindAsync(id);
            _context.UserProfile.Remove(userProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfile.Any(e => e.Id == id);
        }
    }
}
