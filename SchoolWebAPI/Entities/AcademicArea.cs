namespace SchoolWebAPI.Entities
{
    public class AcademicArea
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Course> Courses { get; } = new();
    }
}
