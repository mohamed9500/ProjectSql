using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Data;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            var classes = await _context.Classes
                .Include(c => c.Trainer)
                .ToListAsync();
            return View(classes);
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var @class = await _context.Classes
                .Include(c => c.Trainer)
                .Include(c => c.Enrollments)
                    .ThenInclude(e => e.Member)
                .FirstOrDefaultAsync(m => m.ClassID == id);

            if (@class == null) return NotFound();

            return View(@class);
        }

        // GET: Classes/Create
        public async Task<IActionResult> Create()
        {
            var trainers = await _context.Trainers.ToListAsync();
            // Create a list with FullName for display
            var trainerList = trainers.Select(t => new { 
                t.TrainerID, 
                FullName = t.First_name + " " + t.Last_name 
            }).ToList();
            ViewBag.Trainers = trainerList.Any() 
                ? new SelectList(trainerList, "TrainerID", "FullName") 
                : new SelectList(new List<object>(), "TrainerID", "FullName");
            
            return View();
        }

        // POST: Classes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var trainersList = await _context.Trainers.ToListAsync();
            var trainerList = trainersList.Select(t => new { 
                t.TrainerID, 
                FullName = t.First_name + " " + t.Last_name 
            }).ToList();
            ViewBag.Trainers = trainerList.Any() 
                ? new SelectList(trainerList, "TrainerID", "FullName", @class.TrainerID) 
                : new SelectList(new List<object>(), "TrainerID", "FullName");
            return View(@class);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null) return NotFound();

            var trainers = await _context.Trainers.ToListAsync();
            var trainerList = trainers.Select(t => new { 
                t.TrainerID, 
                FullName = t.First_name + " " + t.Last_name 
            }).ToList();
            ViewBag.Trainers = trainerList.Any() 
                ? new SelectList(trainerList, "TrainerID", "FullName", @class.TrainerID) 
                : new SelectList(new List<object>(), "TrainerID", "FullName");
            return View(@class);
        }

        // POST: Classes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Class @class)
        {
            if (id != @class.ClassID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.ClassID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var trainers = await _context.Trainers.ToListAsync();
            var trainerList = trainers.Select(t => new { 
                t.TrainerID, 
                FullName = t.First_name + " " + t.Last_name 
            }).ToList();
            ViewBag.Trainers = trainerList.Any() 
                ? new SelectList(trainerList, "TrainerID", "FullName", @class.TrainerID) 
                : new SelectList(new List<object>(), "TrainerID", "FullName");
            return View(@class);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @class = await _context.Classes
                .Include(c => c.Trainer)
                .FirstOrDefaultAsync(m => m.ClassID == id);

            if (@class == null) return NotFound();

            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @class = await _context.Classes
                .Include(c => c.Enrollments)
                .FirstOrDefaultAsync(c => c.ClassID == id);
            
            if (@class != null)
            {
                try
                {
                    _context.Classes.Remove(@class);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Class deleted successfully!";
                }
                catch (DbUpdateException)
                {
                    TempData["Error"] = "Cannot delete class. There may be related records.";
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
            return _context.Classes.Any(e => e.ClassID == id);
        }
    }
}

