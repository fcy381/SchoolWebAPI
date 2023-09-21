using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Repositories.Student.Base;
using SchoolWebAPI.Repositories.UnitOfWork.Base;
using System.Runtime.CompilerServices;

namespace SchoolWebAPI.Repositories.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly DbContext _context;
        public IStudentRepository StudentRepository { get; }

        public UnitOfWork(MyDataContext context, IStudentRepository studentRepository) 
        {
            StudentRepository = studentRepository;
            _context = context;
        }

        public async Task<int> Commit()
        => await _context.SaveChangesAsync();

        public void Dispose()
        { 
           _context.Dispose();
        }
    }
}
