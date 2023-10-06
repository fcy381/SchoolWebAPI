namespace SchoolWebAPI.Models.Student
{
    public class StudentGetDTO
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
    }
}
