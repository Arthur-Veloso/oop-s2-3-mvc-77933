using Microsoft.AspNetCore.Identity;
using VgcCollege.Domain.Models;

namespace VgcCollege.mvc.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAndUsers(IServiceProvider serviceProvider)
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
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            // FACULTY
            var facultyEmail = "faculty@vgc.com";
            var faculty = await userManager.FindByEmailAsync(facultyEmail);

            if (faculty == null)
            {
                faculty = new IdentityUser { UserName = facultyEmail, Email = facultyEmail, EmailConfirmed = true };
                await userManager.CreateAsync(faculty, "Faculty123!");
                await userManager.AddToRoleAsync(faculty, "Faculty");
            }

            // STUDENT 1
            var student1Email = "student1@vgc.com";
            var student1 = await userManager.FindByEmailAsync(student1Email);

            if (student1 == null)
            {
                student1 = new IdentityUser { UserName = student1Email, Email = student1Email, EmailConfirmed = true };
                await userManager.CreateAsync(student1, "Student123!");
                await userManager.AddToRoleAsync(student1, "Student");
            }

            // STUDENT 2
            var student2Email = "student2@vgc.com";
            var student2 = await userManager.FindByEmailAsync(student2Email);

            if (student2 == null)
            {
                student2 = new IdentityUser { UserName = student2Email, Email = student2Email, EmailConfirmed = true };
                await userManager.CreateAsync(student2, "Student123!");
                await userManager.AddToRoleAsync(student2, "Student");
            }
        }

        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            if (context.Branches.Any()) return;

            // BRANCHES
            var dublin = new Branch { Name = "Dublin", Address = "Dublin City" };
            var cork = new Branch { Name = "Cork", Address = "Cork City" };

            context.Branches.AddRange(dublin, cork);
            await context.SaveChangesAsync();

            // COURSES
            var course1 = new Course
            {
                Name = "Software Development",
                BranchId = dublin.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(6)
            };

            var course2 = new Course
            {
                Name = "Business Management",
                BranchId = cork.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(6)
            };

            context.Courses.AddRange(course1, course2);
            await context.SaveChangesAsync();

            // STUDENTS
            var student1 = new StudentProfile
            {
                Name = "John Doe",
                Email = "student1@vgc.com",
                Phone = "123456",
                Address = "Dublin",
                StudentNumber = "S001"
            };

            var student2 = new StudentProfile
            {
                Name = "Jane Smith",
                Email = "student2@vgc.com",
                Phone = "654321",
                Address = "Cork",
                StudentNumber = "S002"
            };

            context.Students.AddRange(student1, student2);
            await context.SaveChangesAsync();

            // FACULTY
            var faculty = new FacultyProfile
            {
                Name = "Dr. Brown",
                Email = "faculty@vgc.com",
                Phone = "999999"
            };

            context.Faculty.Add(faculty);
            await context.SaveChangesAsync();

            // ENROLMENTS
            var enrol1 = new CourseEnrolment
            {
                StudentProfileId = student1.Id,
                CourseId = course1.Id,
                EnrolDate = DateTime.Now,
                Status = "Active"
            };

            var enrol2 = new CourseEnrolment
            {
                StudentProfileId = student2.Id,
                CourseId = course2.Id,
                EnrolDate = DateTime.Now,
                Status = "Active"
            };

            context.Enrolments.AddRange(enrol1, enrol2);
            await context.SaveChangesAsync();

            // ASSIGNMENTS
            var assignment = new Assignment
            {
                CourseId = course1.Id,
                Title = "OOP Assignment",
                MaxScore = 100,
                DueDate = DateTime.Now.AddDays(7)
            };

            context.Assignments.Add(assignment);
            await context.SaveChangesAsync();

            // EXAM
            var exam = new Exam
            {
                CourseId = course1.Id,
                Title = "Final Exam",
                Date = DateTime.Now.AddDays(30),
                MaxScore = 100,
                ResultsReleased = true
            };

            context.Exams.Add(exam);
            await context.SaveChangesAsync();

            // EXAM RESULT
            var result = new ExamResult
            {
                ExamId = exam.Id,
                StudentProfileId = student1.Id,
                Score = 85,
                Grade = "A"
            };

            context.ExamResults.Add(result);
            await context.SaveChangesAsync();
        }
    }
}
