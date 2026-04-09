using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgcCollege.Domain.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; } = "";

        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public string StudentNumber { get; set; } = "";

        public ICollection<CourseEnrolment> Enrolments { get; set; } = new List<CourseEnrolment>();
    }
}
