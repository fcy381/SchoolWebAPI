using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Repositories.Student.Base;

namespace SchoolWebAPI.Repositories.Student
{
    public class StudentRepository: SchoolWebAPI.Repositories.GenericRepository.GenericRepository<SchoolWebAPI.Entities.Student>, IStudentRepository
    {
        public StudentRepository(MyDataContext dbContext): base(dbContext)
        {                
        }

        public async Task<SchoolWebAPI.Entities.Student?> GetByIdWithInscriptions(int id) 
            => await Entities.Include(i => i.Inscriptions)
                                .ThenInclude(o => o.OpenCourse)
                                    .ThenInclude(c => c.Course)
                            .Include(i => i.Inscriptions)
                                .ThenInclude(o => o.OpenCourse)
                                    .ThenInclude(t => t.Teacher)
                            .Where(s => s.Id == id).FirstOrDefaultAsync();
    }
}
