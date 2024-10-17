using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;

namespace EventsWebApplication.DataAccess.Repositories
{
    public interface IMemberRepository:IRepository<MemberEntity>
    {
        Task<bool> Delete(Guid eventId, Guid memberId);
        Task<List<MemberEntity>> GetByEventAsync(Guid eventId);
        Task<bool> AlreadyExist(Guid memberId);
        Task<bool> IsMemberInEvent(Guid eventId, Guid memberId);
        Task<bool> AddAsync(Guid eventId, Guid userId, DateOnly registrDate);
    }
}