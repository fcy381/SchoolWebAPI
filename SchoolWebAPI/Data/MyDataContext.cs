using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SchoolWebAPI.Entities;

namespace SchoolWebAPI.Data
{
    public class MyDataContext: DbContext
    {
        public MyDataContext(DbContextOptions<MyDataContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //    base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasMany(e => e.Teachers)
                .WithMany(e => e.Courses)
                .UsingEntity<OpenCourse>(
                    l => l.HasOne<Teacher>().WithMany(e => e.OpenCourses).HasForeignKey(e => e.TeacherId),
                    r => r.HasOne<Course>().WithMany(e => e.OpenCourses).HasForeignKey(e => e.CourseId));

            modelBuilder.Entity<Student>()
                .HasMany(e => e.OpenCourses)
                .WithMany(e => e.Students)
                .UsingEntity<Inscription>(
                    l => l.HasOne<OpenCourse>().WithMany(e => e.Inscriptions).HasForeignKey(e => e.TeacherId ).HasForeignKey(e => e.CourseId),                                        
                    s => s.HasOne<Student>().WithMany(e => e.Inscriptions).HasForeignKey(e => e.StudentId));

            ////    modelBuilder.Entity<Course>()
            ////        .HasMany(e => e.Teachers)
            ////        .WithMany(e => e.Courses)
            ////        .UsingEntity<OpenCourse>(
            ////            l => l.HasOne<Teacher>().WithMany().HasForeignKey(e => e.TeacherId),
            ////            r => r.HasOne<Course>().WithMany().HasForeignKey(e => e.CourseId));

            ////    modelBuilder.Entity<Inscription>()
            ////        .HasKey(k => new { k.CourseId, k.TeacherId, k.StudentId });
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<OpenCourse> OpenCourses { get; set; }

        public DbSet<Inscription> Inscriptions { get; set; }
    }    
}
