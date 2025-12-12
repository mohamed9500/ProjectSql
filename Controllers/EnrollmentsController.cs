using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymManagementSystem.Data;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var enrollments = await _context.Enrollments
                .Include(e => e.Member)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Trainer)
                .OrderByDescending(e => e.EnrolledOn)
                .ToListAsync();

            return View(enrollments);
        }

        // GET: Enrollments/Create
        public async Task<IActionResult> Create()
        {
            // إنشاء قائمة الأعضاء مع عرض الاسم الكامل
            var members = await _context.Members
                .Select(m => new
                {
                    m.MemberID,
                    FullName = m.First_name + " " + m.Last_name
                })
                .ToListAsync();

            ViewBag.Members = new SelectList(members, "MemberID", "FullName");

            // إنشاء قائمة الفصول
            var classes = await _context.Classes.ToListAsync();
            ViewBag.Classes = new SelectList(classes, "ClassID", "ClassName");

            return View();
        }

        // POST: Enrollments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                // التحقق من أن العضو لم يسجل في نفس الفصل من قبل
                var existingEnrollment = await _context.Enrollments
                    .FirstOrDefaultAsync(e => e.MemberID == enrollment.MemberID
                                           && e.ClassID == enrollment.ClassID);

                if (existingEnrollment != null)
                {
                    ModelState.AddModelError("", "This member is already enrolled in this class!");

                    var members = await _context.Members
                        .Select(m => new
                        {
                            m.MemberID,
                            FullName = m.First_name + " " + m.Last_name
                        })
                        .ToListAsync();

                    var classes = await _context.Classes.ToListAsync();
                    ViewBag.Members = new SelectList(members, "MemberID", "FullName", enrollment.MemberID);
                    ViewBag.Classes = new SelectList(classes, "ClassID", "ClassName", enrollment.ClassID);

                    return View(enrollment);
                }

                // تعيين تاريخ التسجيل الحالي
                enrollment.EnrolledOn = DateTime.Now;

                _context.Add(enrollment);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Member enrolled successfully!";
                return RedirectToAction(nameof(Index));
            }

            // في حالة وجود خطأ، إعادة تحميل القوائم
            var membersList = await _context.Members
                .Select(m => new
                {
                    m.MemberID,
                    FullName = m.First_name + " " + m.Last_name
                })
                .ToListAsync();

            var classesList = await _context.Classes.ToListAsync();
            ViewBag.Members = new SelectList(membersList, "MemberID", "FullName", enrollment.MemberID);
            ViewBag.Classes = new SelectList(classesList, "ClassID", "ClassName", enrollment.ClassID);

            return View(enrollment);
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Member)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Trainer)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);

            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var enrollment = await _context.Enrollments
                .Include(e => e.Member)
                .Include(e => e.Class)
                    .ThenInclude(c => c.Trainer)
                .FirstOrDefaultAsync(m => m.EnrollmentID == id);

            if (enrollment == null) return NotFound();

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);

            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Enrollment removed successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // دالة مساعدة للتحقق من وجود التسجيل
        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentID == id);
        }
    }
}
