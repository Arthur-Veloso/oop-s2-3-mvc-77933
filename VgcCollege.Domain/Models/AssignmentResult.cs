using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgcCollege.Domain.Models
{
    public class AssignmentResult
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile? Student { get; set; }

        public int Score { get; set; }

        public string Feedback { get; set; } = "";
    }
}
