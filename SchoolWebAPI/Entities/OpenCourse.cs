namespace SchoolWebAPI.Entities
{
    public class OpenCourse
    {
        public int CourseId { get; set; }

        public int TeacherId { get; set; }

        public Course Course { get; set; } = null!; 

        public Teacher Teacher { get; set; } = null!;

        public List<Student> Students { get; } = new();

        public List<Inscription> Inscriptions { get; } = new(); 
    }
}