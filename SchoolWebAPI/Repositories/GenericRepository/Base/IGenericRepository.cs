﻿using SchoolWebAPI.Entities.Base;

namespace SchoolWebAPI.Repositories.GenericRepository.Base
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> Create(T entity);

        Task<T?> GetById(int id);

        Task<bool?> WasSoftDeleted(T entity);

        IQueryable<T> GetAll();

        void Update(T entity);

        Task<bool> HardDelete(int id);

        Task<bool> SoftDelete(int id);       
    }
}
