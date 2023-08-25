namespace SchoolWebAPI.Entities
{
    public class Inscription
    {
        public int CourseId { get; set; }

        public int TeacherId { get; set; }
        
        public int StudentId { get; set; }

        public Student Student { get; set; } = null!;

        public OpenCourse OpenCourse { get; set; } = null!;        
    }
}
