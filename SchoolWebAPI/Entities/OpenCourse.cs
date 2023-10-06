using SchoolWebAPI.Entities.Base;

namespace SchoolWebAPI.Entities
{
    public class OpenCourse : BaseEntity
    {
        public Guid CourseId { get; set; }

        public Guid TeacherId { get; set; }

        public Course Course { get; set; } = null!; 

        public Teacher Teacher { get; set; } = null!;

        public List<Student> Students { get; } = new();

        public List<Inscription> Inscriptions { get; } = new(); 
    }
}