using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Entities;

namespace SchoolWebAPI.Data
{
    public class MyDataContext: DbContext
    {
        public MyDataContext(DbContextOptions<MyDataContext> options): base(options) { }

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Dictation> Dictations { get; set; }

        public DbSet<Take> Takes { get; set; }
    }    
}
