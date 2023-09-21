using SchoolWebAPI.Entities.Base;
using SchoolWebAPI.Repositories.Student.Base;

namespace SchoolWebAPI.Repositories.UnitOfWork.Base
{
    public interface IUnitOfWork: IDisposable 
    {
        public IStudentRepository StudentRepository { get; }

        Task<int> Commit();
    }
}
