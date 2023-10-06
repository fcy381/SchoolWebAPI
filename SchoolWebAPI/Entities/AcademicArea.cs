using SchoolWebAPI.Entities.Base;

namespace SchoolWebAPI.Entities
{
    public class AcademicArea : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Course> Courses { get; } = new();
    }
}
