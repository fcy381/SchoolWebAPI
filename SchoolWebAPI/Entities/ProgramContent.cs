using SchoolWebAPI.Entities.Base;

namespace SchoolWebAPI.Entities
{
    public class ProgramContent : BaseEntity
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public Guid CourseId { get; set; }

        public Course Course { get; set;} = null!;
    }
}
