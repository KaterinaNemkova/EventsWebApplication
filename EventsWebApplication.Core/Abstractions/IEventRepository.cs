using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.Core.Models;


namespace EventsWebApplication.DataAccess.Repositories
{
    public interface IEventRepository
    {
        Task<bool> AlreadyExist(Guid eventId);
        Task<Guid> Create(Event eventModel);
        Task Delete(Guid id);
        Task<List<EventEntity>> Get();
        Task<List<EventEntity>> GetByFilter(DateTime? eventDate, string? place, EventsCategory? category);
        Task<EventEntity?> GetById(Guid id);
        Task<EventEntity?> GetByName(string title);
        Task Update(Event eventModel);
        Task UploadImage(Guid eventId, string fileName);
    }
}