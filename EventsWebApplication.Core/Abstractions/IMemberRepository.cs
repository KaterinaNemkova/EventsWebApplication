

using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.DataAccess.Repositories
{
    public interface IMemberRepository
    {
        Task<bool> Delete(Guid eventId, Guid memberId);
        Task<List<MemberEntity>> Get(Guid eventId);
        Task<MemberEntity?> GetById(Guid id);

        Task<bool> AlreadyExist(Guid memberId);
        Task<bool> IsMemberInEvent(Guid eventId, Guid memberId);
        Task<bool> Add(Guid eventId, Guid userId, string surname, DateOnly birthDate, DateOnly registrDate);
    }
}