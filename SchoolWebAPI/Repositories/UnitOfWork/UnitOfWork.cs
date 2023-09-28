using Microsoft.EntityFrameworkCore;
using SchoolWebAPI.Data;
using SchoolWebAPI.Repositories.Student.Base;
using SchoolWebAPI.Repositories.UnitOfWork.Base;
using System.Runtime.CompilerServices;

namespace SchoolWebAPI.Repositories.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly MyDataContext _myDataContext;
        public IStudentRepository StudentRepository { get; }

        public UnitOfWork(MyDataContext myDataContext, IStudentRepository studentRepository) 
        {
            StudentRepository = studentRepository;
            _myDataContext = myDataContext;
        }

        public async Task<int> Commit()
        => await _myDataContext.SaveChangesAsync();

        public void Dispose()
        {
            _myDataContext.Dispose();
        }
    }
}
