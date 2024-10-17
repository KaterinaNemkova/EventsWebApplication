using EventsWebApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace EventsWebApplication.DataAccess.Repositories
{
    public class EventRepository :  Repository<EventEntity>,IEventRepository
    {
        public EventRepository(EventsApplicationDbContext context):base(context) { }
        

        public async Task<EventEntity?> GetByNameAsync(string title )
        {
            return await _entities
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Title == title);

        }
        public async Task UploadImage(Guid eventId, string fileName)
        {
            await _context.Events
                .Where(x => x.Id == eventId)
                .ExecuteUpdateAsync(setter => setter
                .SetProperty(e => e.EventImage, fileName));
        }
    }

}
