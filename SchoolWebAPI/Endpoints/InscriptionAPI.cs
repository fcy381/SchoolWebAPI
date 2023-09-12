using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.Inscription;

namespace SchoolWebAPI.Endpoints
{
    public static class InscriptionAPI
    {
        //--------------------------
        // -- Inscription API ------
        //--------------------------

        public static RouteGroupBuilder MapInscriptionAPI(this RouteGroupBuilder group)
        {
            group.MapPost("/course/{courseId}/teacher/{teacherId}/student/{studentId}", async (int courseId, int teacherId, int studentId, MyDataContext db, IMapper mapper) =>
            {
                //Verificar antes si ya existe esa terna.

                var student = await db.Students.FindAsync(studentId);

                if (student == null) return Results.BadRequest();

                var openCourse = db.OpenCourses.Include(c => c.Course).Include(c => c.Teacher).Where(t => (t.TeacherId == teacherId) && (t.CourseId == courseId)).FirstOrDefault();

                if (openCourse == null) return Results.BadRequest();

                var inscription = new Inscription();

                inscription.CourseId = courseId;
                inscription.TeacherId = teacherId;
                inscription.StudentId = studentId;
                inscription.OpenCourse = openCourse;
                inscription.Student = student;

                db.Inscriptions.Add(inscription);
                await db.SaveChangesAsync();

                var inscriptionCreated = mapper.Map<InscriptionGetDTO>(inscription);

                return Results.CreatedAtRoute("", new { courseId = inscription.CourseId, teacherId = inscription.TeacherId, studentId = inscription.StudentId }, inscriptionCreated);
            })
                 .WithName("CreateInscription")
                 .WithTags("Inscription API");

            group.MapGet("/course/{courseId}/teacher/{teacherId}/student/{studentId}", async (int courseId, int teacherId, int studentId, MyDataContext db, IMapper mapper) =>
            {
                var inscription = db.Inscriptions.Include(s => s.Student).Include(oc => oc.OpenCourse).Include(c => c.OpenCourse.Course).Include(t => t.OpenCourse.Teacher).Where(t => (t.CourseId == courseId) && (t.TeacherId == teacherId) && (t.StudentId == studentId)).FirstOrDefault();

                if (inscription == null) return Results.BadRequest();

                var inscriptionGetDTO = mapper.Map<InscriptionGetDTO>(inscription);

                return Results.Ok(inscriptionGetDTO);
            })
                .WithName("GetInscriptionByIds")
                .WithTags("Inscription API");

            group.MapDelete("/course/{courseId}/teacher/{teacherId}/student/{studentId}", async (int courseId, int teacherId, int studentId, MyDataContext db) =>
            {
                var inscription = await db.Inscriptions.FindAsync(courseId, teacherId, studentId);

                if (inscription == null) return Results.NotFound();
                else
                {
                    db.Inscriptions.Remove(inscription);

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("DeleteInscriptionById")
                .WithTags("Inscription API");

            return group;
        }
    }
}
