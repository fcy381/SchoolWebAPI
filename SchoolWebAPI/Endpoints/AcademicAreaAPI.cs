using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.AcademiArea;
using SchoolWebAPI.Models.Course;

namespace SchoolWebAPI.Endpoints
{
    public static class AcademicAreaAPI
    {
        //--------------------------
        // -- Academic Area API ----
        //--------------------------

        public static RouteGroupBuilder MapAcademicAreaAPI(this RouteGroupBuilder group)
        {
            group.MapPost("/initialize", async (MyDataContext db) =>
            {
                var firstAcademicArea = new AcademicArea
                {
                    Name = "Área Ciencias Informáticas",
                };
                await db.AcademicAreas.AddAsync(firstAcademicArea);

                var secondAcademicArea = new AcademicArea
                {
                    Name = "Área Ciencias Matemáticas",
                };
                await db.AcademicAreas.AddAsync(secondAcademicArea);

                await db.SaveChangesAsync();

                return Results.Ok();
            })
                .WithName("CreateFirstAcademicsAreas")
                .WithTags("Academic Area API");

            group.MapPost("/AcademicArea", async (AcademiAreaPostDTO academicAreaPostDTO, MyDataContext db, IMapper mapper) =>
            {
                var academicArea = mapper.Map<AcademicArea>(academicAreaPostDTO);

                await db.AcademicAreas.AddAsync(academicArea);
                await db.SaveChangesAsync();

                var academicAreaCreated = mapper.Map<AcademicAreaGetDTO>(academicArea);

                return Results.CreatedAtRoute("GetAcademicAreaById", new { id = academicArea.Id }, academicAreaCreated);
            })
                .WithName("CreateAcademicArea")
                .WithTags("Academic Area API");

            //group.MapPost("/{academicAreaId}/course/{courseId}", async (int academicAreaId, int courseId, MyDataContext db) =>
            //{
            //    var academicArea = await db.AcademicAreas.Include(c => c.Courses).SingleOrDefaultAsync(i => i.Id == academicAreaId);
            //    if (academicArea == null) return Results.NotFound();
            //    else
            //    {
            //        var course = await db.Courses.FindAsync(courseId);
            //        if (course == null) return Results.NotFound();
            //        else
            //        {
            //            academicArea.Courses.Add(course);
            //            await db.SaveChangesAsync();

            //            return Results.Ok();
            //        }
            //    }
            //})
            //   .WithName("CreateAcademicAreaCourseByIds");

            group.MapGet("/{id}", async (int id, MyDataContext db, IMapper mapper) =>
            {
                var academicArea = await db.AcademicAreas.FindAsync(id);

                if (academicArea == null) return Results.NotFound();
                else
                {
                    var academicAreaGetDTO = new AcademicAreaGetDTO();

                    academicAreaGetDTO = mapper.Map<AcademicAreaGetDTO>(academicArea);

                    return Results.Ok(academicAreaGetDTO);
                }
            })
                .WithName("GetAcademicAreaById")
                .WithTags("Academic Area API");

            group.MapGet("/all", async (MyDataContext db, IMapper mapper) =>
            {
                var academicAreas = await db.AcademicAreas.ToListAsync();

                var academicAreasListDTO = new List<AcademicAreaGetDTO>();

                foreach (var aa in academicAreas)
                {
                    var academicAreaGetDTO = new AcademicAreaGetDTO();

                    academicAreaGetDTO = mapper.Map<AcademicAreaGetDTO>(aa);

                    academicAreasListDTO.Add(academicAreaGetDTO);
                }

                return Results.Ok(academicAreasListDTO);
            })
                .WithName("GetAllAcademicsAreas")
                .WithTags("Academic Area API");

            group.MapGet("/{id}/courses", async (int id, MyDataContext db, IMapper mapper) =>
            {
                var academicArea = db.AcademicAreas
                            .Include(i => i.Courses)
                            .Where(s => s.Id == id).FirstOrDefault();

                if (academicArea == null) return Results.NotFound();
                else
                {
                    var courseListGetDTO = new List<CourseGetDTO>();

                    foreach (var course in academicArea.Courses)
                    {
                        var courseGetDTO = new CourseGetDTO();

                        courseGetDTO = mapper.Map<CourseGetDTO>(course);

                        courseListGetDTO.Add(courseGetDTO);
                    }

                    return Results.Ok(courseListGetDTO);
                }
            })
                .WithName("CreateAcademicAreaCoursesById")
                .WithTags("Academic Area API");

            group.MapPut("/{id}", async (int id, AcademiAreaPostDTO academicAreaPostDTO, MyDataContext db, IMapper mapper) =>
            {
                var academicArea = await db.AcademicAreas.FindAsync(id);

                if (academicArea == null) return Results.NotFound();
                else
                {

                    academicArea = mapper.Map<AcademiAreaPostDTO, AcademicArea>(academicAreaPostDTO, academicArea);

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("UpdateAcademicAreaById")
                .WithTags("Academic Area API");

            group.MapDelete("/{id}", async (int id, MyDataContext db) =>
            {
                var academicArea = await db.AcademicAreas.FindAsync(id);

                if (academicArea == null) return Results.NotFound();
                else
                {
                    db.AcademicAreas.Remove(academicArea);

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("DeleteAcademicAreaById")
                .WithTags("Academic Area API");

            return group;
        }
    }
}
