using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Domain.Models;
using VgcCollege.mvc.Data;

namespace VgcCollege.mvc.Controllers
{
    [Authorize(Roles = "Admin,Faculty,Student")]
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
            var attendances = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Course)
                .ToListAsync();

            return View(attendances);
        }

        // STUDENT VIEW
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

        // GET: Create
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Create()
        {
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");

            return View();
        }

        // POST: Create 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                // 🔴 THIS WILL SHOW YOU THE REAL PROBLEM IN TERMINAL
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name", attendance.StudentProfileId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", attendance.CourseId);

            return View(attendance);
        }
    }
    
}