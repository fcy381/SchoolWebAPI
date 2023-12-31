using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities;
using SchoolWebAPI.Models.Student;
using SchoolWebAPI.Models.Course;
using SchoolWebAPI.Models.Teacher;
using SchoolWebAPI.Models.AcademiArea;
using SchoolWebAPI.Models.ProgramContent;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using SchoolWebAPI.Models.OpenCourses;
using SchoolWebAPI.Models.Inscription;
using SchoolWebAPI.Endpoints;
using FluentValidation;
using SchoolWebAPI.Validators;
using SchoolWebAPI.Repositories.Student.Base;
using SchoolWebAPI.Repositories.Student;
using SchoolWebAPI.Repositories.GenericRepository.Base;
using SchoolWebAPI.Repositories.GenericRepository;
using SchoolWebAPI.Repositories.UnitOfWork.Base;
using SchoolWebAPI.Repositories.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDataContext>(opt => opt.UseInMemoryDatabase("Students"));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IValidator<StudentPostDTO>, StudentPostDTOValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//--------------------------
// -- Student API ----------
//--------------------------

app.MapGroup("/api/v1.0/student").MapStudentAPI();

//--------------------------
// -- Teacher API ----------
//--------------------------

app.MapGroup("/api/v1.0/teacher").MapTeacherAPI();

//--------------------------
// -- Course API -----------
//--------------------------

app.MapGroup("/api/v1.0/course").MapCourseAPI();

//--------------------------
// -- Academic Area API ----
//--------------------------

app.MapGroup("/api/v1.0/academicarea").MapAcademicAreaAPI();

//--------------------------
// -- ProgramContent API ---
//--------------------------

app.MapGroup("/api/v1.0/programcontent").MapProgramContentAPI();

//--------------------------
// -- OpenCourse API -------
//--------------------------

app.MapGroup("/api/v1.0/opencourse").MapOpenCourseAPI();

//--------------------------
// -- Inscription API ------
//--------------------------

app.MapGroup("/api/v1.0/inscription").MapInscriptionAPI();

app.Run();



