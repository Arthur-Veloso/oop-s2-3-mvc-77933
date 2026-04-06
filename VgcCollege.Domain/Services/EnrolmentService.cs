using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VgcCollege.Domain.Models;

namespace VgcCollege.Domain.Services
{
    public class EnrolmentService
    {
        public CourseEnrolment EnrolStudent(int studentId, int courseId)
        {
            return new CourseEnrolment
            {
                StudentProfileId = studentId,
                CourseId = courseId,
                EnrolDate = DateTime.UtcNow,
                Status = "Active"
            };
        }
    }
}
