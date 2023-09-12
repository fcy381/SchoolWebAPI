using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.ProgramContent;

namespace SchoolWebAPI.Endpoints
{
    public static class ProgramContentAPI
    {
        //--------------------------
        // -- ProgramContent API ---
        //--------------------------

        public static RouteGroupBuilder MapProgramContentAPI(this RouteGroupBuilder group)
        {
            group.MapPost("/{courseId}", async (int courseId, ProgramContentPostDTO programContentPostDTO, MyDataContext db, IMapper mapper) =>
            {
                var course = await db.Courses.FindAsync(courseId);

                if (course == null) return Results.NotFound();
                else
                {
                    var programContent = mapper.Map<ProgramContent>(programContentPostDTO);

                    // El mapeo anterior no actualiza en el objeto programContent a que course esta asociado. Eso es algo que hace el ORM al 
                    //ejecutar la siguiente instrucción. ESTA ES UNA SITUACIÓN EN DONDE EL ORM ACTUALIZA UN OBJETO RELACIONADO AL QUE SE ESTÁ
                    //ACTUALIZANDO GRACIAS A LAS PROPIEDADES DE NAVEGACIÓN.
                    course.ProgramContent = programContent;

                    await db.ProgramContents.AddAsync(programContent);
                    await db.SaveChangesAsync();

                    var programContentCreated = mapper.Map<ProgramContentGetDTO>(programContent);

                    return Results.CreatedAtRoute("GetProgramContentById", new { id = programContent.Id }, programContentCreated);
                }
            })
                .WithName("CreateProgramContent")
                .WithTags("Program Content API");

            group.MapGet("/{programContentId}", async (int programContentId, MyDataContext db, IMapper mapper) =>
            {
                var programContent = await db.ProgramContents.FindAsync(programContentId);

                if (programContent == null) return Results.NotFound();
                else
                {
                    var programContentGetDTO = mapper.Map<ProgramContentGetDTO>(programContent);

                    return Results.Ok(programContentGetDTO);
                }
            })
                .WithName("GetProgramContentById")
                .WithTags("Program Content API");

            group.MapGet("/all", async (MyDataContext db, IMapper mapper) =>
            {
                var programContents = await db.ProgramContents.ToListAsync();

                var programContentListDTO = new List<ProgramContentGetDTO>();

                foreach (var pc in programContents)
                {
                    var programContentGetDTO = mapper.Map<ProgramContentGetDTO>(pc);

                    programContentListDTO.Add(programContentGetDTO);
                }

                return Results.Ok(programContentListDTO);
            })
                .WithName("GetAllProgramsContents")
                .WithTags("Program Content API");

            group.MapPut("/{programContentId}", async (int programContentId, ProgramContentPostDTO programContentPostDTO, MyDataContext db) =>
            {
                var programContent = await db.ProgramContents.FindAsync(programContentId);

                if (programContent == null) return Results.NotFound();
                else
                {
                    programContent.Description = programContentPostDTO.Description;

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("UpdateProgramContentById")
                .WithTags("Program Content API");

            group.MapDelete("/{programContentId}", async (int programContentId, MyDataContext db) =>
            {
                var programContent = await db.ProgramContents.FindAsync(programContentId);

                if (programContent == null) return Results.NotFound();
                else
                {
                    db.ProgramContents.Remove(programContent);

                    await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("DeleteProgramContentById")
                .WithTags("Program Content API");

            return group;
        }
    }
}
