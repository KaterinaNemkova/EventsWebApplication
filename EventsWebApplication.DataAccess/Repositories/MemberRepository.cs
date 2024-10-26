using EventsWebApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace EventsWebApplication.DataAccess.Repositories
{
    public class MemberRepository : Repository<MemberEntity>, IMemberRepository
    {
        public MemberRepository(EventsApplicationDbContext context) : base(context) { }

        public async Task<bool> AddAsync(Guid eventId, Guid userId,DateOnly registrDate )
        {
            var eventEntity = await _context.Events
                .Include(e => e.Members)
                .FirstAsync(x=>x.Id == eventId);

            var userEntity = await _context.Users
                .FirstAsync(p => p.Id == userId);

            var memberEntity = new MemberEntity
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                Surname = userEntity.Surname,
                BirthDate = userEntity.BirthDate,
                RegistrationDate = registrDate,
                Email = userEntity.Email,
            };

            await _context.Members.AddAsync(memberEntity);

            if (!eventEntity.Members.Contains(memberEntity))
            {
                eventEntity.Members.Add(memberEntity);
                return true;
            }

            return false;
        }

        public async Task<List<MemberEntity>> GetByEventAsync(Guid eventId, int pageNumber, int pageSize)
        {
            
            return await _context.Events
                .AsNoTracking()  
                .Where(e => e.Id == eventId)
                .SelectMany(e => e.Members)
                .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

        }

        public async Task<bool> Delete(Guid eventId, Guid memberId)
        {
            var eventEntity = await _context.Events
                .Include(m=>m.Members)
                .FirstAsync(e => e.Id == eventId);

            var memberEntity = eventEntity.Members
                .First(m => m.Id == memberId);

            eventEntity.Members.Remove(memberEntity);
            _context.Members.Remove(memberEntity);
            
            return true;
        }

        public async Task<bool> AlreadyExist(Guid memberId)
        {
            var memberExist = await _context.Members.AnyAsync(x => x.Id == memberId);

            return memberExist;

        }

        public async Task<bool> IsMemberInEvent(Guid eventId, Guid memberId)
        {
            return await _context.Events
                .AnyAsync(e => e.Id == eventId && e.Members.Any(m => m.Id == memberId));
        }



    }
}
