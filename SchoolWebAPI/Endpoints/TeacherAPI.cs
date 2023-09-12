using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.Course;
using SchoolWebAPI.Models.Teacher;

namespace SchoolWebAPI.Endpoints
{
    public static class TeacherAPI
    {
        //--------------------------
        // -- Teacher API ----------
        //--------------------------

        public static RouteGroupBuilder MapTeacherAPI(this RouteGroupBuilder group)
        {
            group.MapPost("/initialize", async (MyDataContext db) =>
            {
                var firstTeacher = new Teacher
                {
                    Name = "Martín Bernal",
                    Email = "martinbernal@gmail.com",
                    Phone = "291547889"
                };
                await db.Teachers.AddAsync(firstTeacher);

                var secondTeacher = new Teacher
                {
                    Name = "Pablo García",
                    Email = "pablogarcia@gmail.com",
                    Phone = "291542563"
                };
                await db.Teachers.AddAsync(secondTeacher);

                await db.SaveChangesAsync();

                return Results.Ok();
            })
                .WithName("CreateFirstTeachers")
                .WithTags("Teacher API");

            group.MapPost("/", async (TeacherPostDTO teacherPostDTO, MyDataContext db, IMapper mapper) =>
            {
                var teacher = mapper.Map<Teacher>(teacherPostDTO);

                await db.Teachers.AddAsync(teacher);
                await db.SaveChangesAsync();

                var teacherCreated = mapper.Map<TeacherGetDTO>(teacher);

                return Results.CreatedAtRoute("GetTeacherById", new { id = teacher.Id }, teacherCreated);                
            })
                .WithName("CreateTeacher")
                .WithTags("Teacher API");

            group.MapGet("/{id}", async (int id, MyDataContext db, IMapper mapper) =>
            {
                var teacher = await db.Teachers.FindAsync(id);

                if (teacher == null) return Results.NotFound();
                else
                {
                    var teacherGetDTO = new TeacherGetDTO();

                    teacherGetDTO = mapper.Map<TeacherGetDTO>(teacher);

                    return Results.Ok(teacherGetDTO);
                }
            })
                .WithName("GetTeacherById")
                .WithTags("Teacher API");

            group.MapGet("/all", async (MyDataContext db, IMapper mapper) =>
            {
                var teachers = await db.Teachers.ToListAsync();

                var teachersListDTO = new List<TeacherGetDTO>();

                foreach (var teacher in teachers)
                {
                    var teacherGetDTO = new TeacherGetDTO();

                    teacherGetDTO = mapper.Map<TeacherGetDTO>(teacher);

                    teachersListDTO.Add(teacherGetDTO);
                }

                return Results.Ok(teachersListDTO);
            })
                .WithName("GetAllTeachers")
                .WithTags("Teacher API");

            group.MapGet("/{id}/courses", async (int id, MyDataContext db, IMapper mapper) =>
            {
                var teacher = db.Teachers.Include(c => c.Courses).Where(t => t.Id == id).FirstOrDefault();

                if (teacher == null) return Results.NotFound();
                else
                {
                    var coursesListDTO = new List<CourseGetDTO>();

                    foreach (var course in teacher.Courses)
                    {
                        var courseGetDTO = new CourseGetDTO();

                        courseGetDTO = mapper.Map<CourseGetDTO>(course);

                        coursesListDTO.Add(courseGetDTO);
                    }

                    return Results.Ok(coursesListDTO);
                }
            })
                .WithName("GetSTeacherCoursesById")
                .WithTags("Teacher API");

            group.MapPut("/{id}", async (int id, TeacherPostDTO teacherPostDTO, MyDataContext db, IMapper mapper) =>
            {
                var teacher = await db.Teachers.FindAsync(id);

                if (teacher == null) return Results.NotFound();
                else
                {
                    //teacher = mapper.Map<Teacher>(teacherPostDTO);

                    teacher = mapper.Map<TeacherPostDTO, Teacher>(teacherPostDTO, teacher);

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("UpdateTeacherById")
                .WithTags("Teacher API");

            group.MapDelete("/{id}", async (int id, MyDataContext db) =>
            {
                var teacher = await db.Teachers.FindAsync(id);

                if (teacher == null) return Results.NotFound();
                else
                {
                    db.Teachers.Remove(teacher);

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("DeleteTeacherById")
                .WithTags("Teacher API");

            return group;
        }
    }
}
