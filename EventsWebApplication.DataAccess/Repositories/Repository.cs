using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly EventsApplicationDbContext _context;
        protected readonly DbSet<T> _entities;

        public Repository(EventsApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public virtual async Task CreateAsync(T entity)
        {
            await _entities.AddAsync(entity);
           
        }

        public virtual Task UpdateAsync(T entity)
        {
             _entities.Update(entity);
            return Task.CompletedTask;
        }

        public virtual Task Delete(T entity)
        {
            _entities.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id)
        {
            var entity = await _entities.AsNoTracking().FirstOrDefaultAsync(e=>e.Id==id);
            return entity;
        }
    }
}
