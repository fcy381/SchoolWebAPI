using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.Student;
using SchoolWebAPI.Repositories.Student.Base;

namespace SchoolWebAPI.Endpoints
{
    //--------------------------
    // -- Student API ----------
    //--------------------------

    public static class StudentAPI
    {
        public static RouteGroupBuilder MapStudentAPI(this RouteGroupBuilder group)
        {
            group.MapPost("/initialize", async (IStudentRepository studentRepository) =>
            {
                var firstStudent = new Student
                {
                    Name = "Sebastián Montemaggiore",
                    Email = "sebastianmontemaggiore@gmail.com",
                    Phone = "291544512"
                };
                await studentRepository.Create(firstStudent);
                //await db.Students.AddAsync(firstStudent);

                var secondStudent = new Student
                {
                    Name = "Esteban Gonzalez",
                    Email = "estebangonzalez@gmail.com",
                    Phone = "291544536"
                };
                await studentRepository.Create(secondStudent);
                //await db.Students.AddAsync(secondStudent);

                studentRepository.SaveChanges();
                //await db.SaveChangesAsync();

                return Results.Ok();
            })
                .WithName("CreateFirstStudents")
                .WithTags("Student API");

            //group.MapPost("/", async (StudentPostDTO studentPostDTO, MyDataContext db, IMapper mapper) =>
            //{               
            //    var student = mapper.Map<Student>(studentPostDTO);

            //    await db.Students.AddAsync(student);
            //    await db.SaveChangesAsync();

            //    var studentCreated = mapper.Map<StudentGetDTO>(student);

            //    return Results.CreatedAtRoute("GetStudentById", new { id = student.Id }, studentCreated);                
            //})
            //    .WithName("CreateStudent")
            //    .WithTags("Student API");           

            group.MapPost("/", async (IValidator<StudentPostDTO> validator, 
                                      StudentPostDTO studentPostDTO, 
                                      IMapper mapper,
                                      IStudentRepository studentRepository) => 
            {                               
                var validationResult = await validator.ValidateAsync(studentPostDTO);
                if (!validationResult.IsValid) 
                { 
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var student = mapper.Map<Student>(studentPostDTO);

                await studentRepository.Create(student);
                studentRepository.SaveChanges();

                //await db.Students.AddAsync(student);
                //await db.SaveChangesAsync();

                var studentCreated = mapper.Map<StudentGetDTO>(student);

                return Results.CreatedAtRoute("GetStudentById", new { id = student.Id }, studentCreated);

            })
                .WithName("CreateStudent")
                .WithTags("Student API");

            group.MapGet("/{id}", async (int id, 
                                         IMapper mapper,
                                         IStudentRepository studentRepository) =>
            {
                var student = await studentRepository.GetById(id);
                //var student = await db.Students.FindAsync(id);

                if (student == null) return Results.NotFound();
                else
                {
                    var studentGetDTO = new StudentGetDTO();

                    studentGetDTO = mapper.Map<StudentGetDTO>(student);

                    return Results.Ok(studentGetDTO);
                }
            })
                .WithName("GetStudentById")
                .WithTags("Student API");

            group.MapGet("/all", (IMapper mapper,
                                  IStudentRepository studentRepository) =>
            {
                var students = studentRepository.GetAll();
                //var students = await db.Students.ToListAsync();

                var studentsListDTO = new List<StudentGetDTO>();

                foreach (var stdt in students)
                {
                    var studentGetDTO = new StudentGetDTO();

                    studentGetDTO = mapper.Map<StudentGetDTO>(stdt);

                    studentsListDTO.Add(studentGetDTO);
                }

                return Results.Ok(studentsListDTO);
            })
                .WithName("GetAllStudent")
                .WithTags("Student API");

            group.MapGet("/{id}/inscriptions", async (int id, MyDataContext db,
                                                      IMapper mapper,
                                                      IStudentRepository studentRepository) =>
            {
                var student = await studentRepository.GetByIdWithInscriptions(id); 

                //var student = await db.Students
                //            .Include(i => i.Inscriptions)
                //                .ThenInclude(o => o.OpenCourse)
                //                    .ThenInclude(c => c.Course)
                //            .Include(i => i.Inscriptions)
                //                .ThenInclude(o => o.OpenCourse)
                //                    .ThenInclude(t => t.Teacher)
                //            .Where(s => s.Id == id).FirstOrDefaultAsync();

                if (student == null) return Results.NotFound();
                else
                {
                    var studentInscriptionsGetDTO = new List<StudentInscriptionGetDTO>();

                    foreach (var inscription in student.Inscriptions)
                    {
                        var studentInscriptionGetDTO = mapper.Map<StudentInscriptionGetDTO>(inscription);

                        studentInscriptionsGetDTO.Add(studentInscriptionGetDTO);
                    }

                    return Results.Ok(studentInscriptionsGetDTO);
                }
            })
                .WithName("GetStudentInscriptionsById")
                .WithTags("Student API");

            group.MapPut("/{id}", async (int id, 
                                   StudentPostDTO studentPostDTO, 
                                   IMapper mapper,
                                   IStudentRepository studentRepository) =>
            {
                var student = await studentRepository.GetById(id);
                //var student = await db.Students.FindAsync(id);

                if (student == null) return Results.NotFound();
                else
                {
                    // Lo siguiente crea un nuevo objeto y luego hace un mapeo.
                    //student = mapper.Map<Student>(studentPostDTO);

                    // Para que no se cree un nuevo objeto debo utilizar la siguiente sobrecarga del método Map.
                    student = mapper.Map<StudentPostDTO, Student>(studentPostDTO, student);

                    studentRepository.SaveChanges();
                    //await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("UpdateStudentById")
                .WithTags("Student API");

            group.MapDelete("/{id}", async (int id,
                                            IStudentRepository studentRepository) =>
            {
                var student = await studentRepository.GetById(id);
                //var student = await db.Students.FindAsync(id);

                if (student == null) return Results.NotFound();
                else
                {
                    await studentRepository.SoftDelete(id);
                    //db.Students.Remove(student);

                    studentRepository.SaveChanges();
                    //await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("DeleteStudentById")
                .WithTags("Student API");

            return group;
        }
    }
}
