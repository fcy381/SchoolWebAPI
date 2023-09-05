using AutoMapper;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.AcademiArea;
using SchoolWebAPI.Models.Course;
using SchoolWebAPI.Models.Inscription;
using SchoolWebAPI.Models.Student;
using SchoolWebAPI.Models.Teacher;

namespace SchoolWebAPI.Mappers
{
    public class AutoMapping :Profile
    {
        public AutoMapping() 
        {
            CreateMap<AcademiAreaPostDTO, AcademicArea>();
            CreateMap<AcademicArea, AcademicAreaGetDTO>();

            CreateMap<CoursePostDTO, Course>();
            CreateMap<Course, CourseGetDTO>();

            CreateMap<TeacherPostDTO, Teacher>();
            CreateMap<Teacher, TeacherGetDTO>();

            CreateMap<StudentPostDTO, Student>();
            CreateMap<Student, StudentGetDTO>();

            //CreateMap<Course,Teacher, StudentInscriptionGetDTO>();
        }
    }
}
