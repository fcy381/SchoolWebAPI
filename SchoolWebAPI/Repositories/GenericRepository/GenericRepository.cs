using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolWebAPI.Data;
using SchoolWebAPI.Entities.Base;
using SchoolWebAPI.Repositories.GenericRepository.Base;

namespace SchoolWebAPI.Repositories.GenericRepository
{
    public abstract class GenericRepository<T>: IGenericRepository<T> where T : BaseEntity  
    {
        private readonly MyDataContext _myDataContext;
        protected DbSet<T> Entities => _myDataContext.Set<T>();

        public GenericRepository(MyDataContext myDataContext)
        {
            _myDataContext = myDataContext;
        }

        public async Task<T> Create(T entity)
        {
            EntityEntry<T> insertedValue = await _myDataContext.Set<T>().AddAsync(entity);
            return insertedValue.Entity;
        }

        public async Task<T?> GetById(params object[] keys)
        {
            var entity = await _myDataContext.Set<T>().FindAsync(keys);

            if (entity != null)
                if (entity.IsDeleted == false)
                {
                    return entity;
                };

            return null;
        }

        public async Task<bool?> WasSoftDeleted(T entity)
        { 
            if (entity != null) { return entity.IsDeleted; }
            else { return null; };           
        }

        public IQueryable<T> GetAll()
            => _myDataContext.Set<T>().Where(x => x.IsDeleted == false);

        public IQueryable<T> GetAllEvenThoseSoftDeleted()
            => _myDataContext.Set<T>().Where(x => x.IsDeleted == false);

        public void Update(T entity) 
            => _myDataContext.Set<T>().Update(entity);
        
        public async Task<bool> HardDelete(Guid id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return false;

            _myDataContext.Set<T>().Remove(entity);
            return true;
        }

        public async Task<bool> SoftDelete(Guid id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedTimeUtc = DateTime.UtcNow;

            _myDataContext.Set<T>().Update(entity);
            
            return true;
        }
    }
}
