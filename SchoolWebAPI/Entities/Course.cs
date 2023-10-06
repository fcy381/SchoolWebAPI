using SchoolWebAPI.Entities.Base;
using System.Reflection.Metadata;

namespace SchoolWebAPI.Entities
{
    public class Course : BaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
        
        public List<Teacher> Teachers { get; } = new();

        public List<OpenCourse> OpenCourses { get; } = new();

        public ProgramContent?   ProgramContent { get; set; }

        public Guid? AcademicAreaId { get; set; } 

        public AcademicArea? AcademicArea { get; set; } 
    }
}
