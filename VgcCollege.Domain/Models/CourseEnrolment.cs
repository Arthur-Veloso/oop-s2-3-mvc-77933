using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgcCollege.Domain.Models
{
    public class CourseEnrolment
    {
        public int Id { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile? Student { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public DateTime EnrolDate { get; set; }

        public string Status { get; set; } = "Active";
    }
}
