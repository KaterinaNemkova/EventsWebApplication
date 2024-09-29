using EventsWebApplication.Core.Enums;
using EventsWebApplication.Core.Models;

namespace EventsWebApplication.Application.Services
{
    public interface IMemberService
    {
        Task<AddToEventResult> AddToEvent(Guid eventId, Guid userId, string surname, DateOnly birthDate, DateOnly registrDate);
        Task<DeleteMemberResults> DeleteMember(Guid eventId, Guid memberId);
        Task<List<Member>> Get(Guid id);
        Task<Member?> GetById(Guid id);
        
    }
}