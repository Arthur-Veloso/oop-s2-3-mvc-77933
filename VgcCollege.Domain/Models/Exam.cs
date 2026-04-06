using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgcCollege.Domain.Models
{
    public class Exam
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public string Title { get; set; } = "";

        public DateTime Date { get; set; }

        public int MaxScore { get; set; }

        public bool ResultsReleased { get; set; } = false;
    }
}
