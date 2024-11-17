using Corely.DataAccess.Interfaces.Entities;
using Corely.DataAccess.Interfaces.Repos;
using System.Linq.Expressions;

namespace Corely.DataAccess.Mock.Repos
{
    public class MockRepo<T>
        : IRepo<T>
        where T : class, IHasIdPk
    {
        public readonly List<T> Entities = [];

        public MockRepo() : base() { }

        public virtual Task<int> CreateAsync(T entity)
        {
            Entities.Add(entity);
            return Task.FromResult(entity.Id);
        }

        public virtual Task<T?> GetAsync(int id)
        {
            return Task.FromResult(Entities.FirstOrDefault(u => u.Id == id));
        }

        public virtual async Task<T?> GetAsync(
            Expression<Func<T, bool>> query,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            ArgumentNullException.ThrowIfNull(query);
            var predicate = query.Compile();
            var queryable = Entities.AsQueryable();

            if (include != null)
            {
                queryable = include(queryable);
            }

            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }

            return await Task.FromResult(queryable.FirstOrDefault(predicate));
        }

        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> query)
        {
            ArgumentNullException.ThrowIfNull(query);
            var predicate = query.Compile();
            return Task.FromResult(Entities.Any(predicate));
        }

        public virtual Task<List<T>> ListAsync(
            Expression<Func<T, bool>>? query = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            var queryable = Entities.AsQueryable();
            if (query != null)
            {
                var predicate = query.Compile();
                queryable = queryable.Where(predicate).AsQueryable();
            }
            if (include != null)
            {
                queryable = include(queryable);
            }
            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }
            return Task.FromResult(queryable.ToList());
        }

        public virtual Task UpdateAsync(T entity)
        {
            if (typeof(IHasModifiedUtc).IsAssignableFrom(typeof(T)))
            {
                ((IHasModifiedUtc)entity).ModifiedUtc = DateTime.UtcNow;
            }

            var index = Entities.FindIndex(u => u.Id == entity.Id);
            if (index > -1) { Entities[index] = entity; }
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(T entity)
        {
            Entities.Remove(entity);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(int id)
        {
            Entities.RemoveAll(u => u.Id == id);
            return Task.CompletedTask;
        }
    }
}
