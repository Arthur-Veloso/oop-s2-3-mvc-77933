using VgcCollege.Domain.Services;
using VgcCollege.Domain.Models;
using Xunit;

namespace VgcCollege.Tests
{
    public class AcademicServiceTests
    {
        private readonly AcademicService _service = new AcademicService();

        [Fact]
        public void CalculateGrade_ShouldReturnA()
        {
            var result = _service.CalculateGrade(85, 100);
            Assert.Equal("A", result);
        }

        [Fact]
        public void CalculateGrade_ShouldReturnB()
        {
            var result = _service.CalculateGrade(65, 100);
            Assert.Equal("B", result);
        }

        [Fact]
        public void CalculateGrade_ShouldReturnF()
        {
            var result = _service.CalculateGrade(30, 100);
            Assert.Equal("F", result);
        }

        [Fact]
        public void IsValidScore_ShouldReturnTrue()
        {
            var result = _service.IsValidScore(50, 100);
            Assert.True(result);
        }

        [Fact]
        public void IsValidScore_ShouldReturnFalse_WhenTooHigh()
        {
            var result = _service.IsValidScore(150, 100);
            Assert.False(result);
        }

        [Fact]
        public void CanStudentViewResult_ShouldReturnTrue_WhenReleased()
        {
            var exam = new Exam { ResultsReleased = true };

            var result = _service.CanStudentViewResult(exam);

            Assert.True(result);
        }

        [Fact]
        public void CanStudentViewResult_ShouldReturnFalse_WhenNotReleased()
        {
            var exam = new Exam { ResultsReleased = false };

            var result = _service.CanStudentViewResult(exam);

            Assert.False(result);
        }
    }
}
