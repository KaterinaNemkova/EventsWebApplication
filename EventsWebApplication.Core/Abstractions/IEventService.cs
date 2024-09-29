using EventsWebApplication.Core.Contracts;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.Core.Models;
using Microsoft.AspNetCore.Http;

namespace EventsWebApplication.Application.Services
{
    public interface IEventService
    {
        Task<Guid> CreateEvent(Event eventModel);
        Task Delete(Guid id);
        Task<bool> Exist(Guid id);
        Task<List<Event>> GetByFilter(DateTime? dateTime, string? place, EventsCategory? eventsCategory);
        Task<Event?> GetById(Guid id);
        Task<Event?> GetByName(string name);
        Task<List<Event>> GetEvents();
        Task Update(Guid id,EventsRequest request);
        Task UploadImage(Guid id, IFormFile file);
    }
}