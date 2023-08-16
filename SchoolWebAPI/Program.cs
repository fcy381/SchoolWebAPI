using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDataContext>(opt => opt.UseInMemoryDatabase("Student"));

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

// Student API.

app.MapPost("/Student", async (Student student, MyDataContext db) => 
{ 
    db.Students.Add(student);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/Student/{id}", async (int id, MyDataContext db) => 
{
    var student = await db.Students.FindAsync(id);

    if (student != null) return Results.Ok(student);
    else return Results.NotFound();    
});

app.MapGet("/Student", async (MyDataContext db) => await db.Students.ToListAsync());

app.MapPut("/Student/{id}", async (int id, Student newDataStudent, MyDataContext db) => 
{ 
   var student = await db.Students.FindAsync(id);
    if (student == null) return Results.NotFound();
    else 
    { 
        student.Name = newDataStudent.Name;
        student.Email = newDataStudent.Email;
        student.Phone = newDataStudent.Phone;

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

// Course API.

app.MapPost("/Course", async (Course course, MyDataContext db) =>
{
    db.Courses.Add(course);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/Course/{id}", async (int id, MyDataContext db) =>
{
    var course = await db.Courses.FindAsync(id);

    if (course != null) return Results.Ok(course);
    else return Results.NotFound();
});

app.MapGet("/Course", async (MyDataContext db) => await db.Courses.ToListAsync());

app.MapPut("/Course/{id}", async (int id, Course newDataCourse, MyDataContext db) =>
{
    var course = await db.Courses.FindAsync(id);
    if (course == null) return Results.NotFound();
    else
    {
        course.Name = newDataCourse.Name;
        course.Code = newDataCourse.Code;
        course.Description = newDataCourse.Description;

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});

app.MapDelete("/Course/{id}", async (int id, MyDataContext db) =>
{
    var course = await db.Teachers.FindAsync(id);
    if (course == null) return Results.NotFound();
    else
    {
        db.Teachers.Remove(course);

        await db.SaveChangesAsync();

        return Results.Ok();
    };

});

// Teacher API.

app.MapPost("/Teacher", async ([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] Teacher teacher, MyDataContext db) =>
{
    db.Teachers.Add(teacher);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapGet("/Teacher/{id}", GetTeacher);

static async Task<IResult> GetTeacher(int id, MyDataContext db)
{
    var teacher = await db.Teachers.FindAsync(id);

    if (teacher != null) return Results.Ok(teacher);
    else return Results.NotFound();
}

app.MapGet("/Teacher", async (MyDataContext db) => await db.Teachers.ToListAsync());

app.MapPut("/Teacher/{id}", async (int id, Teacher newDataTeacher, MyDataContext db) =>
{
    var teacher = await db.Teachers.FindAsync(id);
    if (teacher == null) return Results.NotFound();
    else
    {
        teacher.Name = newDataTeacher.Name;
        teacher.Email = newDataTeacher.Email;
        teacher.Phone = newDataTeacher.Phone;

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

// Dictation API.

app.MapPost("/Dictation", async (DTODictation dtoDictation, MyDataContext db) =>
{
    var course = await db.Courses.FindAsync(dtoDictation.CourseId);

    if (course == null) return Results.BadRequest();
    
    var teacher = await db.Teachers.FindAsync(dtoDictation.TeacherId);

    if (teacher == null) return Results.BadRequest();

    Dictation dictation = new Dictation();

    dictation.CourseId = dtoDictation.CourseId;
    dictation.TeacherId = dtoDictation.TeacherId;
    dictation.Course = course;
    dictation.Teacher = teacher;

    db.Dictations.Add(dictation);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapDelete("/Student/{idCourse}/{idTeacher}  ", async (int idCourse, int idTeacher, MyDataContext db) =>
{
    var dictation = await db.Dictations.FindAsync(idCourse, idTeacher);
    if (dictation == null) return Results.NotFound();
    else
    {
        db.Dictations.Remove(dictation);

        await db.SaveChangesAsync();

        return Results.Ok();
    };
});


// Take API.

app.MapPost("/Take", async (DTOTake dtoTake, MyDataContext db) =>
{
    var course = await db.Courses.FindAsync(dtoTake.CourseId);

    if (course == null) return Results.BadRequest();

    var teacher = await db.Teachers.FindAsync(dtoTake.TeacherId);

    if (teacher == null) return Results.BadRequest();

    var student = await db.Students.FindAsync(dtoTake.StudentId);

    if (student == null) return Results.BadRequest();

    Take take = new Take();

    take.CourseId = dtoTake.CourseId;
    take.TeacherId = dtoTake.TeacherId;
    take.Course = course;
    take.Teacher = teacher;

    var students = new List<Student>();

    students.Add(student);

    take.Students = students;

    db.Takes.Add(take);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapDelete("/Student/{idCourse}/{idTeacher}", async (int idCourse, int idTeacher, MyDataContext db) =>
{
    var dictation = await db.Dictations.FindAsync(idCourse, idTeacher);
    if (dictation == null) return Results.NotFound();
    else
    {
        db.Dictations.Remove(dictation);

        await db.SaveChangesAsync();

        return Results.Ok();
    }
});


app.Run();

