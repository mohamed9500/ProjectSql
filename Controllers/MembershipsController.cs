using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Data;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    public class MembershipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Memberships
        public async Task<IActionResult> Index()
        {
            var memberships = await _context.Memberships
                .Include(m => m.Members)
                .ToListAsync();
            return View(memberships);
        }

        // GET: Memberships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var membership = await _context.Memberships
                .Include(m => m.Members)
                .FirstOrDefaultAsync(m => m.MembershipID == id);

            if (membership == null) return NotFound();

            return View(membership);
        }

        // GET: Memberships/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Memberships/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Membership membership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membership);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Membership plan created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(membership);
        }

        // GET: Memberships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null) return NotFound();

            return View(membership);
        }

        // POST: Memberships/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Membership membership)
        {
            if (id != membership.MembershipID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membership);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Membership plan updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipExists(membership.MembershipID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(membership);
        }

        // GET: Memberships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var membership = await _context.Memberships
                .Include(m => m.Members)
                .FirstOrDefaultAsync(m => m.MembershipID == id);

            if (membership == null) return NotFound();

            return View(membership);
        }

        // POST: Memberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membership = await _context.Memberships
                .Include(m => m.Members)
                .FirstOrDefaultAsync(m => m.MembershipID == id);

            if (membership != null)
            {
                if (membership.Members != null && membership.Members.Any())
                {
                    TempData["Error"] = "Cannot delete membership plan. There are members using this plan. Please reassign or remove the members first.";
                    return RedirectToAction(nameof(Delete), new { id });
                }

                try
                {
                    _context.Memberships.Remove(membership);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Membership plan deleted successfully!";
                }
                catch (DbUpdateException)
                {
                    TempData["Error"] = "Cannot delete membership plan. There may be related records.";
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipExists(int id)
        {
            return _context.Memberships.Any(e => e.MembershipID == id);
        }
    }
}

