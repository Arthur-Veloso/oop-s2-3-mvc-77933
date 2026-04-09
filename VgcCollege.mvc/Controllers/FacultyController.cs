using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VgcCollege.Domain.Models;
using VgcCollege.mvc.Data;

namespace VgcCollege.mvc.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]

    public class FacultyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacultyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Faculty")]
        public async Task<IActionResult> MyStudents()
        {
            var userEmail = User.Identity.Name;

            // Get current faculty
            var faculty = await _context.Faculty
                .FirstOrDefaultAsync(f => f.Email == userEmail);

            if (faculty == null)
                return NotFound();

            // Get courses assigned to this faculty
            var courseIds = await _context.Courses
                .Where(c => c.FacultyProfileId == faculty.Id)
                .Select(c => c.Id)
                .ToListAsync();

            // Get ONLY students enrolled in those courses
            var students = await _context.Enrolments
                .Include(e => e.Student)
                .Where(e => courseIds.Contains(e.CourseId))
                .Select(e => e.Student)
                .Distinct()
                .ToListAsync();

            return View(students);
        }

        // Existing methods below
        public async Task<IActionResult> Index()
        {
            return View(await _context.Faculty.ToListAsync());
        }

        // GET: Faculty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultyProfile = await _context.Faculty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facultyProfile == null)
            {
                return NotFound();
            }

            return View(facultyProfile);
        }

        // GET: Faculty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Faculty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdentityUserId,Name,Email,Phone")] FacultyProfile facultyProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facultyProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facultyProfile);
        }

        // GET: Faculty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultyProfile = await _context.Faculty.FindAsync(id);
            if (facultyProfile == null)
            {
                return NotFound();
            }
            return View(facultyProfile);
        }

        // POST: Faculty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdentityUserId,Name,Email,Phone")] FacultyProfile facultyProfile)
        {
            if (id != facultyProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facultyProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyProfileExists(facultyProfile.Id))
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
            return View(facultyProfile);
        }

        // GET: Faculty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultyProfile = await _context.Faculty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facultyProfile == null)
            {
                return NotFound();
            }

            return View(facultyProfile);
        }

        // POST: Faculty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facultyProfile = await _context.Faculty.FindAsync(id);
            if (facultyProfile != null)
            {
                _context.Faculty.Remove(facultyProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultyProfileExists(int id)
        {
            return _context.Faculty.Any(e => e.Id == id);
        }
    }
}
