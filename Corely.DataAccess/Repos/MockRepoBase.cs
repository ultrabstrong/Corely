using Corely.Domain.Entities;
using Corely.Domain.Repos;

namespace Corely.DataAccess.Repos
{
    internal abstract class MockRepoBase<T>
        : IRepo<T>
        where T : IHasIdPk
    {
        protected readonly List<T> Entities = [];

        public Task CreateAsync(T entity)
        {
            Entities.Add(entity);
            return Task.CompletedTask;
        }

        public Task<T?> GetAsync(int id)
        {
            return Task.FromResult(Entities.FirstOrDefault(u => u.Id == id));
        }

        public Task UpdateAsync(T entity)
        {
            var index = Entities.FindIndex(u => u.Id == entity.Id);
            if (index > -1) { Entities[index] = entity; }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            Entities.Remove(entity);
            return Task.CompletedTask;
        }
    }
}
