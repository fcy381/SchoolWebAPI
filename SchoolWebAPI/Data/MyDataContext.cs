using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Models;

namespace SchoolWebAPI.Data
{ 
    public class MyDataContext: DbContext
    {
        public MyDataContext(DbContextOptions<MyDataContext> options): base(options) { }

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
    }    
}
