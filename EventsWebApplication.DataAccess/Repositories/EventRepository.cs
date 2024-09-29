using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.Core.Mappers;
using EventsWebApplication.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace EventsWebApplication.DataAccess.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly EventsApplicationDbContext _context;

        public EventRepository(EventsApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<EventEntity>> Get()
        {
            var eventEntities = await _context.Events
                .AsNoTracking()
                .ToListAsync();
            
            return eventEntities;
            
        }

        public async Task<EventEntity?> GetById(Guid id)
        {
            var eventEntity=await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            return eventEntity;
            
        }

        public async Task<EventEntity?> GetByName(string title)
        {
            var eventEntity=await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Title == title);
            
            return eventEntity;


        }

        public async Task<List<EventEntity>> GetByFilter(DateTime? eventDate, string? place, EventsCategory? category)
        {
            var query = _context.Events.AsNoTracking();

            if (!string.IsNullOrEmpty(place))
            {
                query = query.Where(x => x.Place.Contains(place));
            }

            if (eventDate.HasValue)
            {
                query = query.Where(x => x.DateTime.Date == eventDate.Value.Date);
            }

            if (category.HasValue)
            {
                query = query.Where(x => x.EventCategory == category.Value);
            }

            var eventEntities=await query.ToListAsync();
            
            return eventEntities;
        }

        public async Task<Guid> Create(Event eventModel)
        {
            var eventEntity = new EventEntity
            {
                Id = Guid.NewGuid(),
                Title = eventModel.Title,
                Description = eventModel.Description,
                DateTime = eventModel.DateTime,
                Place = eventModel.Place,
                EventCategory = eventModel.EventCategory,
                MaxCountPeople =eventModel.MaxCountPeople,

            };

            await _context.Events.AddAsync(eventEntity);

            await _context.SaveChangesAsync();

            return eventEntity.Id;
        }

        public async Task Update(Event eventModel)
        {
            await _context.Events
                .Where(x => x.Id == eventModel.Id)
                .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Title, eventModel.Title)
                .SetProperty(x => x.Description, eventModel.Description)
                .SetProperty(x => x.DateTime, eventModel.DateTime)
                .SetProperty(x => x.Place, eventModel.Place)
                .SetProperty(x => x.EventCategory, eventModel.EventCategory)
                .SetProperty(x => x.MaxCountPeople, eventModel.MaxCountPeople));
          
        }

        public async Task Delete(Guid id)
        {
            await _context.Events
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<bool> AlreadyExist(Guid eventId)
        {
            var eventExist = await _context.Events.AnyAsync(x => x.Id == eventId);

            return eventExist;
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
