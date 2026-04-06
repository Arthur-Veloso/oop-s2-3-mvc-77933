using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VgcCollege.Domain.Models;

namespace VgcCollege.Domain.Services
{
    public class AcademicService
    {
        // Assignment validation
        public bool IsValidScore(int score, int maxScore)
        {
            return score >= 0 && score <= maxScore;
        }

        // Grade calculation
        public string CalculateGrade(int score, int maxScore)
        {
            double percentage = (double)score / maxScore * 100;

            if (percentage >= 75) return "A";
            if (percentage >= 65) return "B";
            if (percentage >= 50) return "C";
            if (percentage >= 40) return "D";
            return "F";
        }

        //  RULE
        public bool CanStudentViewResult(Exam exam)
        {
            return exam.ResultsReleased;
        }
    }
}
