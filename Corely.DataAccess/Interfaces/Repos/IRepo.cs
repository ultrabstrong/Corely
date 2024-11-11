﻿namespace Corely.DataAccess.Interfaces.Repos
{
    public interface IRepo<T>
    {
        Task<int> CreateAsync(T entity);
        Task<T?> GetAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteAsync(int id);
    }
}
