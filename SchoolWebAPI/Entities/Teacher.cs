using SchoolWebAPI.Entities.Base;

namespace SchoolWebAPI.Entities
{
    public class Teacher : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }        

        public List<Course> Courses { get; } = new();

        public List<OpenCourse> OpenCourses { get; } = new();
    }
}
