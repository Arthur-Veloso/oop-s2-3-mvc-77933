using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VgcCollege.Domain.Models;
using VgcCollege.Domain.Services;
using VgcCollege.mvc.Data;

namespace VgcCollege.mvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseEnrolmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EnrolmentService _service;

        public CourseEnrolmentsController(ApplicationDbContext context)
        {
            _context = context;
            _service = new EnrolmentService();
        }

        // GET: CourseEnrolments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Enrolments
                .Include(c => c.Course)
                .Include(c => c.Student);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CourseEnrolments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseEnrolment = await _context.Enrolments
                .Include(c => c.Course)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (courseEnrolment == null)
            {
                return NotFound();
            }

            return View(courseEnrolment);
        }

        // GET: CourseEnrolments/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name");
            ViewData["StatusList"] = new SelectList(new[] { "Active", "Completed", "Dropped" });
            return View();
        }


        // POST: CourseEnrolments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentProfileId,CourseId")] CourseEnrolment courseEnrolment)
        {
            if (ModelState.IsValid)
            {
                //  Use Domain Service
                var enrolment = _service.EnrolStudent(
                    courseEnrolment.StudentProfileId,
                    courseEnrolment.CourseId
                );

                _context.Enrolments.Add(enrolment);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseEnrolment.CourseId);
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name", courseEnrolment.StudentProfileId);
            ViewData["StatusList"] = new SelectList(new[] { "Active", "Completed", "Dropped" }, courseEnrolment.Status);

            return View(courseEnrolment);
        }

        // GET: CourseEnrolments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseEnrolment = await _context.Enrolments.FindAsync(id);
            if (courseEnrolment == null)
            {
                return NotFound();
            }

            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseEnrolment.CourseId);
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name", courseEnrolment.StudentProfileId);
            ViewData["StatusList"] = new SelectList(new[] { "Active", "Completed", "Dropped" }, courseEnrolment.Status);

            return View(courseEnrolment);
        }

        // POST: CourseEnrolments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentProfileId,CourseId,EnrolDate,Status")] CourseEnrolment courseEnrolment)
        {
            if (id != courseEnrolment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseEnrolment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseEnrolmentExists(courseEnrolment.Id))
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

            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseEnrolment.CourseId);
            ViewData["StudentProfileId"] = new SelectList(_context.Students, "Id", "Name", courseEnrolment.StudentProfileId);
            ViewData["StatusList"] = new SelectList(new[] { "Active", "Completed", "Dropped" }, courseEnrolment.Status);

            return View(courseEnrolment);
        }

        // GET: CourseEnrolments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseEnrolment = await _context.Enrolments
                .Include(c => c.Course)
                .Include(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (courseEnrolment == null)
            {
                return NotFound();
            }

            return View(courseEnrolment);
        }

        // POST: CourseEnrolments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseEnrolment = await _context.Enrolments.FindAsync(id);

            if (courseEnrolment != null)
            {
                _context.Enrolments.Remove(courseEnrolment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CourseEnrolmentExists(int id)
        {
            return _context.Enrolments.Any(e => e.Id == id);
        }
    }
}
