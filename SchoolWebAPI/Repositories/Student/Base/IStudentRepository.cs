using SchoolWebAPI.Repositories.GenericRepository.Base;

namespace SchoolWebAPI.Repositories.Student.Base
{
    public interface IStudentRepository: IGenericRepository<SchoolWebAPI.Entities.Student>
    {
        Task<SchoolWebAPI.Entities.Student?> GetByIdWithInscriptions(Guid id);      
    }
}
