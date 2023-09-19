using SchoolWebAPI.Entities.Base;

namespace SchoolWebAPI.Entities
{
    public class Student: BaseEntity
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public List<OpenCourse> OpenCourses { get; } = new();

        public List<Inscription> Inscriptions { get; } = new();        
    }
}
