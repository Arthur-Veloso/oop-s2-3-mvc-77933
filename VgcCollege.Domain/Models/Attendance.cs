using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgcCollege.Domain.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime Date { get; set; }

        public bool IsPresent { get; set; }
    }
}
