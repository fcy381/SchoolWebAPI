﻿using SchoolWebAPI.Entities.Base;

namespace SchoolWebAPI.Repositories.GenericRepository.Base
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> Create(T entity);

        Task<T?> GetById(params object[] keys);

        Task<bool?> WasSoftDeleted(T entity);

        IQueryable<T> GetAll();

        void Update(T entity);

        Task<bool> HardDelete(Guid id);

        Task<bool> SoftDelete(Guid id);       
    }
}
