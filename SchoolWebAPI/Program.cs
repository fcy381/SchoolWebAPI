using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

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

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
