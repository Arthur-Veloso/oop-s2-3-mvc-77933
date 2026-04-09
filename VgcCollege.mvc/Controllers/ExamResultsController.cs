using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using VgcCollege.Domain.Models;
using VgcCollege.mvc.Data;

namespace VgcCollege.mvc.Controllers
{
    [Authorize(Roles = "Admin,Student")]
    public class ExamResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ STUDENT RESULTS
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity.Name;

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == userEmail);

            if (student == null)
                return NotFound();

            var results = await _context.ExamResults
                .Include(r => r.Exam)
                    .ThenInclude(e => e.Course) // 🔥 IMPORTANT FIX
                .Where(r => r.StudentProfileId == student.Id
                            && r.Exam.ResultsReleased == true)
                .ToListAsync();

            return View(results);
        }

        // GET: ExamResults/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var examResult = await _context.ExamResults
                .Include(e => e.Exam)
                    .ThenInclude(e => e.Course) // 🔥 FIX
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (examResult == null)
                return NotFound();

            return View(examResult);
        }

        // GET: ExamResults/Create
        public IActionResult Create()
        {
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title");
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name"); // FIXED
            return View();
        }

        // POST: ExamResults/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ExamId,StudentProfileId,Score,Grade")] ExamResult examResult)
        {
            if (ModelState.IsValid)
            {
                _context.Add(examResult);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title", examResult.ExamId);
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name", examResult.StudentProfileId);

            return View(examResult);
        }

        // GET: ExamResults/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var examResult = await _context.ExamResults.FindAsync(id);

            if (examResult == null)
                return NotFound();

            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title", examResult.ExamId);
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name", examResult.StudentProfileId);

            return View(examResult);
        }

        // POST: ExamResults/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExamId,StudentProfileId,Score,Grade")] ExamResult examResult)
        {
            if (id != examResult.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(examResult);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamResultExists(examResult.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Title", examResult.ExamId);
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name", examResult.StudentProfileId);

            return View(examResult);
        }

        // GET: ExamResults/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var examResult = await _context.ExamResults
                .Include(e => e.Exam)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (examResult == null)
                return NotFound();

            return View(examResult);
        }

        // POST: ExamResults/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var examResult = await _context.ExamResults.FindAsync(id);

            if (examResult != null)
                _context.ExamResults.Remove(examResult);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ExamResultExists(int id)
        {
            return _context.ExamResults.Any(e => e.Id == id);
        }
    }
}