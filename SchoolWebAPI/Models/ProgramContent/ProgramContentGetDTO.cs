namespace SchoolWebAPI.Models.ProgramContent
{
    public class ProgramContentGetDTO
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public int CourseId { get; set; }
    }
}
