using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgcCollege.Domain.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }

        public int CourseEnrolmentId { get; set; }
        public CourseEnrolment? Enrolment { get; set; }

        public int WeekNumber { get; set; }

        public bool Present { get; set; }
    }
}
