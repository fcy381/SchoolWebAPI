using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.OpenCourses;

namespace SchoolWebAPI.Endpoints
{
    public static class OpenCourseAPI
    {
        //--------------------------
        // -- OpenCourse API -------
        //--------------------------

        public static RouteGroupBuilder MapOpenCourseAPI(this RouteGroupBuilder group)
        {
            group.MapPost("/course/{courseId}/teacher/{teacherId}", async (int courseId, int teacherId, MyDataContext db, IMapper mapper) =>
            {
                // Vertificar antes si ya existe esa dupla.

                var course = await db.Courses.FindAsync(courseId);

                if (course == null) return Results.BadRequest();

                var teacher = await db.Teachers.FindAsync(teacherId);

                if (teacher == null) return Results.BadRequest();

                OpenCourse openCourse = new OpenCourse();

                openCourse.Course = course;
                openCourse.Teacher = teacher;
                openCourse.CourseId = courseId;
                openCourse.TeacherId = teacherId;

                db.OpenCourses.Add(openCourse);
                await db.SaveChangesAsync();

                var openCourseCreated = mapper.Map<OpenCourseGetDTO>(openCourse);

                return Results.CreatedAtRoute("GetOpenCourseByIds", new { courseId = openCourse.CourseId, teacherId = openCourse.TeacherId }, openCourseCreated);
            })
                .WithName("CreateOpenCourse")
                .WithTags("Open Course API");

            group.MapGet("/course/{courseId}/teacher/{teacherId}", async (int courseId, int teacherId, MyDataContext db, IMapper mapper) =>
            {
                var openCourse = db.OpenCourses.Include(c => c.Course).Include(c => c.Teacher).Where(t => (t.TeacherId == teacherId) && (t.CourseId == courseId)).FirstOrDefault();

                if (openCourse == null) return Results.BadRequest();

                var openCourseGetDTO = mapper.Map<OpenCourseGetDTO>(openCourse);

                return Results.Ok(openCourseGetDTO);
            })
                .WithName("GetOpenCourseByIds")
                .WithTags("Open Course API");

            group.MapDelete("/course/{courseId}/teacher/{teacherId}", async (int courseId, int teacherId, MyDataContext db) =>
            {
                var openCourse = await db.OpenCourses.FindAsync(courseId, teacherId);

                if (openCourse == null) return Results.NotFound();
                else
                {
                    db.OpenCourses.Remove(openCourse);

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("DeleteOpenCourseByIds")
                .WithTags("Open Course API");

            return group;
        }
    }
}
