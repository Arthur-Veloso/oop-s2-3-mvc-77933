using VgcCollege.Domain.Services;
using Xunit;

namespace VgcCollege.Tests
{
    public class EnrolmentServiceTests
    {
        [Fact]
        public void EnrolStudent_ShouldCreateValidEnrolment()
        {
            var service = new EnrolmentService();

            var result = service.EnrolStudent(1, 2);

            Assert.Equal(1, result.StudentProfileId);
            Assert.Equal(2, result.CourseId);
            Assert.Equal("Active", result.Status);
        }

        [Fact]
        public void EnrolStudent_ShouldSetCurrentDate()
        {
            var service = new EnrolmentService();

            var result = service.EnrolStudent(1, 1);

            Assert.True(result.EnrolDate <= DateTime.UtcNow);
        }

        [Fact]
        public void EnrolStudent_StatusShouldBeActive()
        {
            var service = new EnrolmentService();

            var result = service.EnrolStudent(5, 10);

            Assert.Equal("Active", result.Status);
        }
    }
}
