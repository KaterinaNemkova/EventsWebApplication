using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;



namespace EventsWebApplication.DataAccess.Repositories
{
    public interface IEventRepository:IRepository<EventEntity>
    {
        Task<EventEntity?> GetByNameAsync(string title);
        Task<int> GetTotalCountAsync();
        Task UploadImage(Guid eventId, string fileName);
    }
}