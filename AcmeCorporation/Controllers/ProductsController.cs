
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AcmeCorporation.Areas.Identity.Data;
using AcmeCorporation.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AcmeCorporation.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DrawContext _context;

        public ProductsController(DrawContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var drawContext = _context.Product.Include(p => p.ProductSerialNumber).Include(p => p.UserProfile);
            return View(await drawContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductSerialNumber)
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [ValidateAntiForgeryToken]
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductSerialNumberId"] = new SelectList(_context.Set<ProductSerialNumbers>(), "Id", "Id");
            ViewData["UserProfileId"] = new SelectList(_context.UserProfile, "Id", "FirstName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,Price,ProductSerialNumberId,UserProfileId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductSerialNumberId"] = new SelectList(_context.Set<ProductSerialNumbers>(), "Id", "Id", product.ProductSerialNumberId);
            ViewData["UserProfileId"] = new SelectList(_context.UserProfile, "Id", "FirstName", product.UserProfileId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductSerialNumberId"] = new SelectList(_context.Set<ProductSerialNumbers>(), "Id", "Id", product.ProductSerialNumberId);
            ViewData["UserProfileId"] = new SelectList(_context.UserProfile, "Id", "FirstName", product.UserProfileId);
            return View(product);
        }
        public async Task<IActionResult> Purchase(int? id, string user)
        {
            var prod = await _context.Product
                    .Include(p => p.UserProfile)
                    .FirstOrDefaultAsync(m => m.Id == id);

            IdentityUser s = new IdentityUser();


            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user);
            var userProfile = await _context.UserProfile.FirstOrDefaultAsync(up => up.Id == currentUser.UserProfileId);

            userProfile.UserProducts = new List<Product>();
            userProfile.UserProducts.Add(prod);


            int userProdId = 0;
            foreach (var item in userProfile.UserProducts)
            {
                userProdId = item.Id;
            }

            // 
            var local = _context.Set<UserProfile>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(userProdId));

            // check if local is not null 
            if (local != null)
            {
                // detach
                _context.Entry(local).State = EntityState.Detached;
            }

            _context.UserProfile.Attach(userProfile).State = EntityState.Modified;

            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
            // POST: Products/Edit/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductName,Price,ProductSerialNumberId,UserProfileId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductSerialNumberId"] = new SelectList(_context.Set<ProductSerialNumbers>(), "Id", "Id", product.ProductSerialNumberId);
            ViewData["UserProfileId"] = new SelectList(_context.UserProfile, "Id", "FirstName", product.UserProfileId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProductSerialNumber)
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
