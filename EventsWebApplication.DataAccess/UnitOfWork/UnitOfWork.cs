

using EventsWebApplication.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventsApplicationDbContext _context;


        private IUserRepository? _userRepository;
        private IMemberRepository? _memberRepository;
        private IEventRepository? _eventRepository;

       
        public UnitOfWork(EventsApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository userRepository => _userRepository ??= new UserRepository(_context);
        public IMemberRepository memberRepository => _memberRepository ??= new MemberRepository(_context);
        public IEventRepository eventRepository => _eventRepository ??= new EventRepository(_context);
        public async Task SaveChangesAsync( )
        {
            await _context.SaveChangesAsync( );
        }
    }
}
