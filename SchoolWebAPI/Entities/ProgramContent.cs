namespace SchoolWebAPI.Entities
{
    public class ProgramContent
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set;} = null!;
    }
}
