

using EventsWebApplication.DataAccess.Repositories;

namespace EventsWebApplication.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        IMemberRepository memberRepository { get; }
        IEventRepository eventRepository { get; }
        Task SaveChangesAsync();
    }
}
