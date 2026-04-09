using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Domain.Models;

namespace VgcCollege.mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<StudentProfile> Students => Set<StudentProfile>();
        public DbSet<FacultyProfile> Faculty => Set<FacultyProfile>();
        public DbSet<CourseEnrolment> Enrolments => Set<CourseEnrolment>();
        public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<AssignmentResult> AssignmentResults => Set<AssignmentResult>();
        public DbSet<Exam> Exams => Set<Exam>();
        public DbSet<ExamResult> ExamResults => Set<ExamResult>();
        public DbSet<Attendance> Attendances { get; set; }

    }
}
