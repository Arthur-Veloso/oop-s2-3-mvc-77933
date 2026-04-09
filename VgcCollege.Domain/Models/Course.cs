using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgcCollege.Domain.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? FacultyProfileId { get; set; }
        public FacultyProfile? Faculty { get; set; }

        public ICollection<CourseEnrolment> Enrolments { get; set; } = new List<CourseEnrolment>();
    }
}
