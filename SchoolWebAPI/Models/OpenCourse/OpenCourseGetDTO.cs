using SchoolWebAPI.Models.Course;
using SchoolWebAPI.Models.Teacher;

namespace SchoolWebAPI.Models.OpenCourses
{
    public class OpenCourseGetDTO
    {        
        public CourseGetDTO Course { get; set; } = null!; 

        public TeacherGetDTO Teacher { get; set; } = null!;
    }
}
