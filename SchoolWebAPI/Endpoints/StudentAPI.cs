using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.Student;
using SchoolWebAPI.Repositories.Student.Base;
using SchoolWebAPI.Repositories.UnitOfWork.Base;

namespace SchoolWebAPI.Endpoints
{
    //--------------------------
    // -- Student API ----------
    //--------------------------

    public static class StudentAPI
    {
        public static RouteGroupBuilder MapStudentAPI(this RouteGroupBuilder group)
        {
            // -- CreateFirstStudents ----------

            group.MapPost("/initialize", async (IUnitOfWork unitOfWork) =>
            {
                var firstStudent = new Student
                {
                    Name = "Sebastián Montemaggiore",
                    Email = "sebastianmontemaggiore@gmail.com",
                    Phone = "291544512"
                };
                await unitOfWork.StudentRepository.Create(firstStudent);
                //await studentRepository.Create(firstStudent);
                //await db.Students.AddAsync(firstStudent);

                var secondStudent = new Student
                {
                    Name = "Esteban Gonzalez",
                    Email = "estebangonzalez@gmail.com",
                    Phone = "291544536"
                };
                await unitOfWork.StudentRepository.Create(secondStudent);
                //await db.Students.AddAsync(secondStudent);

                _ = await unitOfWork.Commit();
                //studentRepository.SaveChanges();
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

            // -- CreateStudent ----------------

            group.MapPost("/", async (IValidator<StudentPostDTO> validator, 
                                      StudentPostDTO studentPostDTO, 
                                      IMapper mapper,
                                      IUnitOfWork unitOfWork) => 
            {                               
                var validationResult = await validator.ValidateAsync(studentPostDTO);
                if (!validationResult.IsValid) 
                { 
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var student = mapper.Map<Student>(studentPostDTO);

                await unitOfWork.StudentRepository.Create(student);
                _ = await unitOfWork.Commit();

                //await db.Students.AddAsync(student);
                //await db.SaveChangesAsync();

                var studentCreated = mapper.Map<StudentGetDTO>(student);

                return Results.CreatedAtRoute("GetStudentById", new { id = student.Id }, studentCreated);

            })
                .WithName("CreateStudent")
                .WithTags("Student API");

            // -- GetStudentById ---------------

            group.MapGet("/{id}", async (int id, 
                                         IMapper mapper,
                                         IUnitOfWork unitOfWork) =>
            {
                var student = await unitOfWork.StudentRepository.GetById(id);
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

            // -- GetAllStudent ----------------

            group.MapGet("/all", (IMapper mapper,
                                  IUnitOfWork unitOfWork) =>
            {
                var students = unitOfWork.StudentRepository.GetAll();
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

            // -- GetStudentInscriptionsById ---

            group.MapGet("/{id}/inscriptions", async (int id, MyDataContext db,
                                                      IMapper mapper,
                                                      IUnitOfWork unitOfWork) =>
            {
                var student = await unitOfWork.StudentRepository.GetByIdWithInscriptions(id); 

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

            // -- UpdateStudentById ------------

            group.MapPut("/{id}", async (int id, 
                                   StudentPostDTO studentPostDTO, 
                                   IMapper mapper,
                                   IUnitOfWork unitOfWork) =>
            {
                var student = await unitOfWork.StudentRepository.GetById(id);
                //var student = await db.Students.FindAsync(id);

                if (student == null) return Results.NotFound();
                else
                {
                    // Lo siguiente crea un nuevo objeto y luego hace un mapeo.
                    //student = mapper.Map<Student>(studentPostDTO);

                    // Para que no se cree un nuevo objeto debo utilizar la siguiente sobrecarga del método Map.
                    student = mapper.Map<StudentPostDTO, Student>(studentPostDTO, student);

                    _ = await unitOfWork.Commit();
                    //await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("UpdateStudentById")
                .WithTags("Student API");

            // -- HardDeleteStudentById --------

            group.MapDelete("/hard/{id}", async (int id,
                                                 IUnitOfWork unitOfWork) =>
            {
                var student = await unitOfWork.StudentRepository.GetById(id);
                //var student = await db.Students.FindAsync(id);

                if (student == null) return Results.NotFound();
                else
                {
                    await unitOfWork.StudentRepository.SoftDelete(id);
                    //db.Students.Remove(student);

                    //unitOfWork.Dispose();
                    _ = await unitOfWork.Commit();
                    //await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("HardDeleteStudentById")
                .WithTags("Student API");

            // -- SoftDeleteStudentById --------

            group.MapDelete("/soft/{id}", async (int id,
                                                 IUnitOfWork unitOfWork) =>
            {
                var student = await unitOfWork.StudentRepository.GetById(id);
                //var student = await db.Students.FindAsync(id);

                if (student == null) return Results.NotFound();
                else
                {
                    await unitOfWork.StudentRepository.SoftDelete(id);
                    //db.Students.Remove(student);

                    _ = await unitOfWork.Commit();
                    //await db.SaveChangesAsync();

                    return Results.Ok();
                };
            })
                .WithName("SoftDeleteStudentById")
                .WithTags("Student API");

            return group;
        }
    }
}
