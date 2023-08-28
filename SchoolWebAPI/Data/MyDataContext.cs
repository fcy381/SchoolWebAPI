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
                    l => l.HasOne<OpenCourse>().WithMany(e => e.Inscriptions).HasForeignKey(e => new { e.CourseId, e.TeacherId }),                                        
                    s => s.HasOne<Student>().WithMany(e => e.Inscriptions).HasForeignKey(e => e.StudentId));

            modelBuilder.Entity<Course>()
                .HasOne(e => e.Content)
                .WithOne(e => e.Course)
                .HasForeignKey<Content>(e => e.CourseId)
                .IsRequired();
                
        }

        public DbSet<Student> Students { get; set; }

       public DbSet<Teacher> Teachers { get; set; }
 
        public DbSet<Course> Courses { get; set; }

        public DbSet<OpenCourse> OpenCourses { get; set; }

        public DbSet<Inscription> Inscriptions { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<AcademicArea> AcademicsAreas { get; set; }
    }    
}
