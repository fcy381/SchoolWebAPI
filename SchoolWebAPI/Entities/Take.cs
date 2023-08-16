namespace SchoolWebAPI.Entities
{
    public class Take
    {
        public int CourseId { get; set; }

        public int TeacherId { get; set; }

        public Course Course { get; set; } = null!;

        public Teacher Teacher { get; set; } = null!;

        public List<Student> Students { get; set; } = new();
    }
}
