using EventsWebApplication.Core.Entities;

namespace EventsWebApplication.Core.Abstractions
{
    public interface IRepository<T> where T : Entity
    {
        Task CreateAsync(T entity);
        Task Delete(T entity);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}