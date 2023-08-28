using SchoolWebAPI.Models.Course;
using SchoolWebAPI.Models.Teacher;

namespace SchoolWebAPI.Models.Student
{
    public class StudentInscriptionGetDTO
    {
        public CourseGetDTO Course { get; set; } = null!;

        public TeacherGetDTO Teacher { get; set; } = null!;
    }
}
