using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.Student;
using SchoolWebAPI.Models.Course;
using SchoolWebAPI.Models.Teacher;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDataContext>(opt => opt.UseInMemoryDatabase("Students"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//--------------------------
// -- Teacher API ----------
//--------------------------

app.MapPost("/Teacher/Inicializar", async (MyDataContext db) =>
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
});

app.MapPost("/Teacher", async (TeacherPostDTO teacherPostDTO, MyDataContext db) =>
{
    var teacher = new Teacher();

    teacher.Name = teacherPostDTO.Name;
    teacher.Email = teacherPostDTO.Email;
    teacher.Phone = teacherPostDTO.Phone;

    await db.Teachers.AddAsync(teacher);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/Teacher/{id}", async (int id, MyDataContext db) =>
{
    var teacher = await db.Teachers.FindAsync(id);

    if (teacher == null) return Results.NotFound();
    else
    {
        var teacherGetDTO = new TeacherGetDTO();

        teacherGetDTO.Id = teacher.Id;
        teacherGetDTO.Name = teacher.Name;
        teacherGetDTO.Email = teacher.Email;
        teacherGetDTO.Phone = teacher.Phone;

        return Results.Ok(teacherGetDTO);
    }
});

app.MapGet("/Teachers", async (MyDataContext db) =>
{
    var teachers = await db.Teachers.ToListAsync();

    var teachersListDTO = new List<TeacherGetDTO>();

    foreach (var teacher in teachers)
    {
        var teacherGetDTO = new TeacherGetDTO();

        teacherGetDTO.Id = teacher.Id;
        teacherGetDTO.Name = teacher.Name;
        teacherGetDTO.Email = teacher.Email;
        teacherGetDTO.Phone = teacher.Phone;

        teachersListDTO.Add(teacherGetDTO);
    }

    return Results.Ok(teachersListDTO);
});

app.MapGet("/Teacher/{id}/Courses", async (int id, MyDataContext db) =>
{
    var teacher = db.Teachers.Include(c => c.Courses).Where(t => t.Id == id).FirstOrDefault();

    if (teacher == null) return Results.NotFound();
    else
    {
        var coursesListDTO = new List<CourseGetDTO>();

        foreach (var course in teacher.Courses)
        {
            var courseGetDTO = new CourseGetDTO();

            courseGetDTO.Id = course.Id;
            courseGetDTO.Name = course.Name;
            courseGetDTO.Code = course.Code;
            courseGetDTO.Description = course.Description;

            coursesListDTO.Add(courseGetDTO);
        }

        return Results.Ok(coursesListDTO);
    }
});

app.MapPut("/Teacher/{id}", async (int id, TeacherPostDTO teacherPostDTO, MyDataContext db) =>
{
    var teacher = await db.Teachers.FindAsync(id);

    if (teacher == null) return Results.NotFound();
    else
    {
        teacher.Name = teacherPostDTO.Name;
        teacher.Email = teacherPostDTO.Email;
        teacher.Phone = teacherPostDTO.Phone;

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});

app.MapDelete("/Teacher/{id}", async (int id, MyDataContext db) =>
{
    var teacher = await db.Teachers.FindAsync(id);

    if (teacher == null) return Results.NotFound();
    else
    {
        db.Teachers.Remove(teacher);

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});

//--------------------------
// -- Course API -----------
//--------------------------

app.MapPost("/Course/Inicializar", async (MyDataContext db) =>
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
});

app.MapPost("/Course", async (CoursePostDTO coursePostDTO, MyDataContext db) =>
{
    var course = new Course();

    course.Name = coursePostDTO.Name;
    course.Code = coursePostDTO.Code;
    course.Description = coursePostDTO.Description;

    await db.Courses.AddAsync(course);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/Course/{id}", async (int id, MyDataContext db) =>
{
    var course = await db.Courses.FindAsync(id);

    if (course == null) return Results.NotFound();
    else
    {
        var courseGetDTO = new CourseGetDTO();

        courseGetDTO.Id = course.Id;
        courseGetDTO.Name = course.Name;
        courseGetDTO.Code = course.Code;
        courseGetDTO.Description = course.Description;

        return Results.Ok(courseGetDTO);
    }
});

app.MapGet("/Courses", async (MyDataContext db) =>
{
    var courses = await db.Courses.ToListAsync();

    var coursesListDTO = new List<CourseGetDTO>();

    foreach (var course in courses)
    {
        var courseGetDTO = new CourseGetDTO();

        courseGetDTO.Id = course.Id;
        courseGetDTO.Name = course.Name;
        courseGetDTO.Code = course.Code;
        courseGetDTO.Description = course.Description;

        coursesListDTO.Add(courseGetDTO);
    }

    return Results.Ok(coursesListDTO);
});

app.MapGet("/Course/{id}/Teachers", async (int id, MyDataContext db) =>
{
    var course = db.Courses.Include(t => t.Teachers).Where(c => c.Id == id).FirstOrDefault();

    if (course == null) return Results.NotFound();
    else
    {
        var teachersListDTO = new List<TeacherGetDTO>();

        foreach (var teacher in course.Teachers)
        {
            var teacherGetDTO = new TeacherGetDTO();

            teacherGetDTO.Id = teacher.Id;
            teacherGetDTO.Name = teacher.Name;
            teacherGetDTO.Email = teacher.Email;
            teacherGetDTO.Phone = teacher.Phone;

            teachersListDTO.Add(teacherGetDTO);
        }

        return Results.Ok(teachersListDTO);
    }
});

app.MapPut("/Course/{id}", async (int id, CoursePostDTO coursePostDTO, MyDataContext db) =>
{
    var course = await db.Courses.FindAsync(id);

    if (course == null) return Results.NotFound();
    else
    {
        course.Name = coursePostDTO.Name;
        course.Code = coursePostDTO.Code;
        course.Description = coursePostDTO.Description;

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});

app.MapDelete("/Course/{id}", async (int id, MyDataContext db) =>
{
    var course = await db.Courses.FindAsync(id);

    if (course == null) return Results.NotFound();
    else
    {
        db.Courses.Remove(course);

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});

//--------------------------
// -- Student API -----------
//--------------------------

app.MapPost("/Student/Inicializar", async (MyDataContext db) =>
{
    var firstStudent = new Student
    {
        Name = "Sebastián Montemaggiore",
        Email = "sebastianmontemaggiore@gmail.com",
        Phone = "291544512"
    };
    await db.Students.AddAsync(firstStudent);

    var secondStudent = new Student
    {
        Name = "Esteban Gonzalez",
        Email = "estebangonzalez@gmail.com",
        Phone = "291544536"
    };
    await db.Students.AddAsync(secondStudent);

    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapPost("/Student", async (StudentPostDTO studentPostDTO, MyDataContext db) => 
{
    var student = new Student();

    student.Name = studentPostDTO.Name;
    student.Email = studentPostDTO.Email;
    student.Phone = studentPostDTO.Phone;

    await db.Students.AddAsync(student);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/Student/{id}", async (int id, MyDataContext db) => 
{
    var student = await db.Students.FindAsync(id);

    if (student == null) return Results.NotFound();
    else
    {
        var studentGetDTO = new StudentGetDTO();

        studentGetDTO.Id = student.Id;
        studentGetDTO.Name = student.Name;
        studentGetDTO.Email = student.Email;
        studentGetDTO.Phone = student.Phone;

        return Results.Ok(studentGetDTO);
    }
});

app.MapGet("/Students", async (MyDataContext db) => 
{
    var students = await db.Students.ToListAsync();

    var studentsListDTO = new List<StudentGetDTO>();

    foreach (var stdt in students) 
    {
        var studentGetDTO = new StudentGetDTO();

        studentGetDTO.Id = stdt.Id;
        studentGetDTO.Name = stdt.Name;
        studentGetDTO.Email = stdt.Email;
        studentGetDTO.Phone = stdt.Phone;

        studentsListDTO.Add(studentGetDTO);
    }

    return Results.Ok(studentsListDTO);
});

//app.MapGet("/Students/{id}/OpenCourses", async (int id, MyDataContext db) => 
//{ 

//});

app.MapPut("/Student/{id}", async (int id, StudentPostDTO studentPostDTO, MyDataContext db) => 
{ 
    var student = await db.Students.FindAsync(id);

    if (student == null) return Results.NotFound();
    else 
    { 
        student.Name = studentPostDTO.Name;
        student.Email = studentPostDTO.Email;
        student.Phone = studentPostDTO.Phone;

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});

app.MapDelete("/Student/{id}", async (int id, MyDataContext db) => 
{ 
    var student = await db.Students.FindAsync(id);

    if (student == null) return Results.NotFound();
    else 
    {
        db.Students.Remove(student);

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});

//--------------------------
// -- OpenCourse API -------
//--------------------------

app.MapPost("/OpenCourse/Course/{courseId}/Teacher/{teacherId}", async (int courseId, int teacherId, MyDataContext db) =>
{
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

    return Results.Ok();
});

app.MapGet("/OpenCourse/Course/{courseId}/Teacher/{teacherId}", async (int courseId, int teacherId, MyDataContext db) =>
{
    var openCourse = db.OpenCourses.Include(c => c.Course).Include(c => c.Teacher).Where(t => (t.TeacherId == teacherId) && (t.CourseId == courseId)).FirstOrDefault(); 

    if (openCourse == null) return Results.BadRequest();

    var openCourseGetDTO = new SchoolWebAPI.Models.OpenCourses.OpenCourseGetDTO();

    var courseGetDTO = new CourseGetDTO();

    courseGetDTO.Id = courseId;
    courseGetDTO.Name = openCourse.Course.Name;
    courseGetDTO.Code = openCourse.Course.Code;
    courseGetDTO.Description = openCourse.Course.Description;
    
    var teacherGetDTO = new TeacherGetDTO();

    teacherGetDTO.Id = teacherId;
    teacherGetDTO.Name = openCourse.Teacher.Name;   
    teacherGetDTO.Email = openCourse.Teacher.Email; 
    teacherGetDTO.Phone = openCourse.Teacher.Phone;

    openCourseGetDTO.Course = courseGetDTO;
    openCourseGetDTO.Teacher = teacherGetDTO;

    return Results.Ok(openCourseGetDTO);
});

app.MapDelete("/OpenCourse/Course/{courseId}/Teacher/{teacherId}", async (int courseId, int teacherId, MyDataContext db) =>
{
    var openCourse = await db.OpenCourses.FindAsync(courseId, teacherId);

    if (openCourse == null) return Results.NotFound();
    else
    {
        db.OpenCourses.Remove(openCourse);

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});

//--------------------------
// -- Inscription API -------
//--------------------------

app.MapPost("/Inscription/Course/{courseId}/Teacher/{teacherId}/Student/{studentId}", async (int courseId, int teacherId, int studentId, MyDataContext db) =>
{
    var course = await db.Courses.FindAsync(courseId);

    if (course == null) return Results.BadRequest();

    var teacher = await db.Teachers.FindAsync(teacherId);

    if (teacher == null) return Results.BadRequest();

    var student = await db.Students.FindAsync(studentId);

    if (student == null) return Results.BadRequest();

    var openCourse = new OpenCourse();

    openCourse.CourseId = courseId;
    openCourse.TeacherId = teacherId;
    openCourse.Course = course;
    openCourse.Teacher = teacher;

    var inscription = new Inscription();
    
    inscription.CourseId = courseId;
    inscription.TeacherId = teacherId;
    inscription.StudentId = studentId;
    inscription.OpenCourse = openCourse;
    inscription.Student = student;

    db.Inscriptions.Add(inscription);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/Inscription/Course/{courseId}/Teacher/{teacherId}/Student/{studentId}", async (int courseId, int teacherId, int studentId, MyDataContext db) =>
{
    var inscription = db.Inscriptions.Include(c => c.OpenCourse).Include(c => c.Student).Where(t => (t.CourseId == courseId) && (t.TeacherId == teacherId) && (t.StudentId == studentId)).FirstOrDefault();

    if (inscription == null) return Results.BadRequest();

    var inscriptionGetDTO = new SchoolWebAPI.Models.Inscription.InscriptionGetDTO();

    inscriptionGetDTO.Course = inscription.OpenCourse.Course;
    inscriptionGetDTO.Teacher = inscription.OpenCourse.Teacher;
    inscriptionGetDTO.Student = inscription.Student;

    return Results.Ok(inscriptionGetDTO);
});

app.MapDelete("/Take/Course/{courseId}/Teacher/{teacherId}/Student/{studentId}", async (int courseId, int teacherId, int studentId, MyDataContext db) =>
{
    var inscription = await db.Inscriptions.FindAsync(courseId, teacherId, studentId);

    if (inscription == null) return Results.NotFound();
    else
    {
        db.Inscriptions.Remove(inscription);

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});

app.Run();

