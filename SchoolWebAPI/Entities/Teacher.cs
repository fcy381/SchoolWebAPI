namespace SchoolWebAPI.Entities
{
    public class Teacher
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public List<Course> Courses { get; set; } = new();
    }
}
