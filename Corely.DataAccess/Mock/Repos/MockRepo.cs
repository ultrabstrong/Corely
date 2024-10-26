using Corely.DataAccess.Interfaces.Entities;
using Corely.DataAccess.Interfaces.Repos;

namespace Corely.DataAccess.Mock.Repos
{
    public class MockRepo<T>
        : IRepo<T>
        where T : IHasIdPk
    {
        protected readonly List<T> Entities = [];

        public Task<int> CreateAsync(T entity)
        {
            Entities.Add(entity);
            return Task.FromResult(entity.Id);
        }

        public virtual Task<T?> GetAsync(int id)
        {
            return Task.FromResult(Entities.FirstOrDefault(u => u.Id == id));
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
    }
}
