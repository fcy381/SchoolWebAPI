using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolWebAPI.Entities.Base;
using SchoolWebAPI.Repositories.GenericRepository.Base;

namespace SchoolWebAPI.Repositories.GenericRepository
{
    public abstract class GenericRepository<T>: IGenericRepository<T> where T : BaseEntity  
    {
        private readonly DbContext _dbContext;
        protected DbSet<T> Entities => _dbContext.Set<T>();

        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> Create(T entity)
        {
            EntityEntry<T> insertedValue = await _dbContext.Set<T>().AddAsync(entity);
            return insertedValue.Entity;
        }

        public async Task<T?> GetById(int id) 
            => await _dbContext.Set<T>()
                .Where(x => x.IsDeleted == false && x.Id == id) 
                .FirstOrDefaultAsync();

        public async Task<bool?> WasSoftDeleted(T entity)
        { 
            if (entity != null) { return entity.IsDeleted; }
            else { return null; };           
        }

        public IQueryable<T> GetAll()
            => _dbContext.Set<T>().Where(x => x.IsDeleted == false);

        public IQueryable<T> GetAllEvenThoseSoftDeleted()
            => _dbContext.Set<T>().Where(x => x.IsDeleted == false);

        public void Update(T entity) 
            => _dbContext.Set<T>().Update(entity);
        
        public async Task<bool> HardDelete(int id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return false;

            _dbContext.Set<T>().Remove(entity);
            return true;
        }

        public async Task<bool> SoftDelete(int id)
        {
            T? entity = await GetById(id);

            if (entity is null)
                return false;

            entity.IsDeleted = true;
            entity.DeletedTimeUtc = DateTime.UtcNow;

            _dbContext.Set<T>().Update(entity);
            
            return true;
        }

        public async void SaveChanges() 
            => await _dbContext.SaveChangesAsync();
    }
}
