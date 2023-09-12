using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.Course;
using SchoolWebAPI.Models.Teacher;

namespace SchoolWebAPI.Endpoints
{
    public static class CourseAPI
    {
        //--------------------------
        // -- Course API -----------
        //--------------------------

        public static RouteGroupBuilder MapCourseAPI(this RouteGroupBuilder group)
        {
            group.MapPost("/initialize", async (MyDataContext db) =>
            {
                var firstCourse = new Course
                {
                    Name = "Git I",
                    Code = "AA1",
                    Description = "Curso que permite comprender los conceptos básicos del control de versiones, y para ello hace uso de un fremework de versionado concreto que es llamado GIT."
                };
                await db.Courses.AddAsync(firstCourse);

                var secondCourse = new Course
                {
                    Name = "C# I",
                    Code = "AA2",
                    Description = "Curso que permite comprender los conceptos básicos del lenguaje C#."
                };
                await db.Courses.AddAsync(secondCourse);

                await db.SaveChangesAsync();

                return Results.Ok();
            })
                .WithName("CreateFirstCourses")
                .WithTags("Course API");

            group.MapPost("/", async (CoursePostDTO coursePostDTO, MyDataContext db, IMapper mapper) =>
            {
                var course = mapper.Map<Course>(coursePostDTO);

                await db.Courses.AddAsync(course);
                await db.SaveChangesAsync();

                var courseCreated = mapper.Map<CourseGetDTO>(course);

                return Results.CreatedAtRoute("GetCourseById", new { id = course.Id }, courseCreated);
            })
                .WithName("CreateCourse")
                .WithTags("Course API"); 

            group.MapGet("/{id}", async (int id, MyDataContext db, IMapper mapper) =>
            {
                var course = await db.Courses.FindAsync(id);

                if (course == null) return Results.NotFound();
                else
                {
                    var courseGetDTO = new CourseGetDTO();

                    courseGetDTO = mapper.Map<CourseGetDTO>(course);

                    return Results.Ok(courseGetDTO);
                }
            })
                .WithName("GetCourseById")
                .WithTags("Course API");

            group.MapGet("/all", async (MyDataContext db, IMapper mapper) =>
            {
                var courses = await db.Courses.ToListAsync();

                var coursesListDTO = new List<CourseGetDTO>();

                foreach (var course in courses)
                {
                    var courseGetDTO = new CourseGetDTO();

                    courseGetDTO = mapper.Map<CourseGetDTO>(course);

                    coursesListDTO.Add(courseGetDTO);
                }

                return Results.Ok(coursesListDTO);
            })
                .WithName("GetAllCourses")
                .WithTags("Course API");

            group.MapGet("/{id}/Teachers", async (int id, MyDataContext db, IMapper mapper) =>
            {
                var course = db.Courses.Include(t => t.Teachers).Where(c => c.Id == id).FirstOrDefault();

                if (course == null) return Results.NotFound();
                else
                {
                    var teachersListDTO = new List<TeacherGetDTO>();

                    foreach (var teacher in course.Teachers)
                    {
                        var teacherGetDTO = new TeacherGetDTO();

                        teacherGetDTO = mapper.Map<TeacherGetDTO>(teacher);

                        teachersListDTO.Add(teacherGetDTO);
                    }

                    return Results.Ok(teachersListDTO);
                }
            })
                .WithName("GetCourseTeachersById")
                .WithTags("Course API");

            group.MapGet("/{id}/ProgramContent", async (int id, MyDataContext db, IMapper mapper) =>
            {
                var course = await db.Courses.Include(c => c.ProgramContent).SingleOrDefaultAsync(i => i.Id == id);

                if (course == null) return Results.NotFound();
                else
                    if (course.ProgramContent != null)
                {
                    var courseProgramContentGetDTO = mapper.Map<CourseProgramContentGetDTO>(course.ProgramContent);

                    return Results.Ok(courseProgramContentGetDTO);
                }
                else return Results.NoContent();
            })
                .WithName("GetCourseProgramContentById")
                .WithTags("Course API");

            group.MapGet("/{id}/AcademicArea", async (int id, MyDataContext db, IMapper mapper) =>
            {
                var course = await db.Courses.Include(c => c.AcademicArea).SingleOrDefaultAsync(i => i.Id == id);

                if (course == null) return Results.NotFound();
                else
                    if (course.AcademicArea != null)
                {
                    var courseAcademicAreaGetDTO = mapper.Map<CourseAcademicAreaGetDTO>(course.AcademicArea);

                    return Results.Ok(courseAcademicAreaGetDTO);
                }
                else return Results.NoContent();
            })
                .WithName("GetCourseAcademicAreaById")
                .WithTags("Course API");

            group.MapPut("/{id}", async (int id, CoursePostDTO coursePostDTO, MyDataContext db, IMapper mapper) =>
            {
                var course = await db.Courses.FindAsync(id);

                if (course == null) return Results.NotFound();
                else
                {
                    //course = mapper.Map<Course>(coursePostDTO);

                    course = mapper.Map<CoursePostDTO, Course>(coursePostDTO, course);

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("UpdateCourseById")
                .WithTags("Course API");

            group.MapDelete("/{id}", async (int id, MyDataContext db) =>
            {
                var course = await db.Courses.FindAsync(id);

                if (course == null) return Results.NotFound();
                else
                {
                    db.Courses.Remove(course);

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("DeleteCourseById")
                .WithTags("Course API");

            return group;
        }
    }
}
