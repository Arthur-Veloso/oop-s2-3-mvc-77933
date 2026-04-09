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
    [Authorize(Roles = "Admin,Faculty, Student")]
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
            var attendance = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Course)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return View(attendance);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyAttendance()
        {
            var userEmail = User.Identity.Name;

            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Email == userEmail);

            if (student == null)
                return NotFound();

            var attendance = await _context.Attendances
                .Include(a => a.Course)
                .Where(a => a.StudentProfileId == student.Id)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            return View(attendance);
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Course)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create()
        {
            // Default (Admin sees everything)
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");

            // If Faculty → filter courses
            if (User.IsInRole("Faculty"))
            {
                var userEmail = User.Identity.Name;

                var faculty = _context.Faculty
                    .FirstOrDefault(f => f.Email == userEmail);

                if (faculty != null)
                {
                    var courseIds = _context.Courses
                        .Where(c => c.FacultyProfileId == faculty.Id)
                        .Select(c => c.Id)
                        .ToList();

                    ViewData["CourseId"] = new SelectList(
                        _context.Courses.Where(c => courseIds.Contains(c.Id)),
                        "Id",
                        "Name"
                    );
                }     
                    

            }

            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StudentProfileId,CourseId,Date,IsPresent")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", attendance.CourseId);
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Address", attendance.StudentProfileId);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", attendance.CourseId);
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Address", attendance.StudentProfileId);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentProfileId,CourseId,Date,IsPresent")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", attendance.CourseId);
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Address", attendance.StudentProfileId);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Course)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}
