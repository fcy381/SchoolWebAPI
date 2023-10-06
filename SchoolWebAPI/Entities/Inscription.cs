using SchoolWebAPI.Entities.Base;

namespace SchoolWebAPI.Entities
{
    public class Inscription : BaseEntity
    {
        public Guid CourseId { get; set; }

        public Guid TeacherId { get; set; }
        
        public Guid StudentId { get; set; }

        public Student Student { get; set; } = null!;

        public OpenCourse OpenCourse { get; set; } = null!;        
    }
}
