namespace SchoolWebAPI.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public List<Dictation> Dictations { get; set; } = new();
    }
}
