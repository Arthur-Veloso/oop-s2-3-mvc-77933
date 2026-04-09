using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Domain.Models;

namespace VgcCollege.mvc.Data
{
    public static class SeedData
    {
        // ========================
        // ROLES & USERS
        // ========================
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Admin", "Faculty", "Student" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // ADMIN
            var adminEmail = "admin@vgc.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // FACULTY USERS (3)
            var facultyEmails = new[] { "faculty@vgc.com", "faculty2@vgc.com", "faculty3@vgc.com" };

            foreach (var email in facultyEmails)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                    await userManager.CreateAsync(user, "Faculty123!");
                    await userManager.AddToRoleAsync(user, "Faculty");
                }
            }

            // STUDENTS
            var studentEmails = new[] { "student1@vgc.com", "student2@vgc.com", "student3@vgc.com" };

            foreach (var email in studentEmails)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var student = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                    await userManager.CreateAsync(student, "Student123!");
                    await userManager.AddToRoleAsync(student, "Student");
                }
            }
        }

        // ========================
        // APPLICATION DATA
        // ========================
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            if (context.Branches.Any()) return;

            // BRANCHES
            var dublin = new Branch { Name = "Dublin", Address = "Dublin City" };
            var cork = new Branch { Name = "Cork", Address = "Cork City" };
            var limerick = new Branch { Name = "Limerick", Address = "Limerick City" };

            context.Branches.AddRange(dublin, cork, limerick);
            await context.SaveChangesAsync();

            // FACULTY (3)
            var faculty1 = new FacultyProfile { Name = "Dr. Brown", Email = "faculty@vgc.com", Phone = "999999" };
            var faculty2 = new FacultyProfile { Name = "Ms. Green", Email = "faculty2@vgc.com", Phone = "888888" };
            var faculty3 = new FacultyProfile { Name = "Mr. White", Email = "faculty3@vgc.com", Phone = "777777" };

            context.Faculty.AddRange(faculty1, faculty2, faculty3);
            await context.SaveChangesAsync();

            // COURSES (ASSIGNED TO FACULTY ✅)
            var courses = new List<Course>
            {
                new Course { Name = "Software Development", BranchId = dublin.Id, FacultyProfileId = faculty1.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) },
                new Course { Name = "Data Science", BranchId = dublin.Id, FacultyProfileId = faculty1.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) },

                new Course { Name = "Business Management", BranchId = cork.Id, FacultyProfileId = faculty2.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) },
                new Course { Name = "Marketing", BranchId = cork.Id, FacultyProfileId = faculty2.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) },

                new Course { Name = "Cyber Security", BranchId = limerick.Id, FacultyProfileId = faculty3.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) },
                new Course { Name = "Networking", BranchId = limerick.Id, FacultyProfileId = faculty3.Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) }
            };

            context.Courses.AddRange(courses);
            await context.SaveChangesAsync();

            // STUDENTS
            var student1 = new StudentProfile { Name = "John Doe", Email = "student1@vgc.com", Phone = "123456", Address = "Dublin", StudentNumber = "S001" };
            var student2 = new StudentProfile { Name = "Jane Smith", Email = "student2@vgc.com", Phone = "654321", Address = "Cork", StudentNumber = "S002" };
            var student3 = new StudentProfile { Name = "Mike Johnson", Email = "student3@vgc.com", Phone = "777777", Address = "Limerick", StudentNumber = "S003" };

            context.Students.AddRange(student1, student2, student3);
            await context.SaveChangesAsync();

            // ENROLMENTS
            var enrolments = new List<CourseEnrolment>
            {
                new CourseEnrolment { StudentProfileId = student1.Id, CourseId = courses[0].Id, EnrolDate = DateTime.Now, Status = "Active" },
                new CourseEnrolment { StudentProfileId = student2.Id, CourseId = courses[2].Id, EnrolDate = DateTime.Now, Status = "Active" },
                new CourseEnrolment { StudentProfileId = student3.Id, CourseId = courses[4].Id, EnrolDate = DateTime.Now, Status = "Active" }
            };

            context.Enrolments.AddRange(enrolments);
            await context.SaveChangesAsync();

            // ASSIGNMENTS
            var assignments = courses.Select((c, i) => new Assignment
            {
                CourseId = c.Id,
                Title = $"Assignment {i + 1}",
                MaxScore = 100,
                DueDate = DateTime.Now.AddDays(7 + i)
            }).ToList();

            context.Assignments.AddRange(assignments);
            await context.SaveChangesAsync();

            // EXAMS
            var exams = courses.Select((c, i) => new Exam
            {
                CourseId = c.Id,
                Title = $"Exam {i + 1}",
                Date = DateTime.Now.AddDays(30 + i),
                MaxScore = 100,
                ResultsReleased = i % 2 == 0
            }).ToList();

            context.Exams.AddRange(exams);
            await context.SaveChangesAsync();

            // RESULTS
            var results = new List<ExamResult>
            {
                new ExamResult { ExamId = exams[0].Id, StudentProfileId = student1.Id, Score = 85, Grade = "A" },
                new ExamResult { ExamId = exams[1].Id, StudentProfileId = student2.Id, Score = 70, Grade = "B" },
                new ExamResult { ExamId = exams[2].Id, StudentProfileId = student3.Id, Score = 60, Grade = "C" }
            };

            context.ExamResults.AddRange(results);
            await context.SaveChangesAsync();
        }
    }
}