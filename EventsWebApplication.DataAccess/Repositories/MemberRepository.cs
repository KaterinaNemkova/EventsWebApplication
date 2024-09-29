using EventsWebApplication.Core.Models;
using EventsWebApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace EventsWebApplication.DataAccess.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly EventsApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MemberRepository(EventsApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<bool> Add(Guid eventId, Guid userId, string surname, DateOnly birthDate, DateOnly registrDate)
        {
            var eventEntity = await _context.Events
                .Include(e => e.Members)
                .FirstAsync(x=>x.Id == eventId);

            var userEntity = await _context.Users
                .FirstAsync(p => p.Id == userId);

            var memberEntity = new MemberEntity
            {
                Id = userEntity.Id,
                Name = userEntity.UserName,
                Surname = surname,
                BirthDate = birthDate,
                RegistrationDate = registrDate,
                Email = userEntity.Email,
            };

            await _context.Members.AddAsync(memberEntity);

            if (!eventEntity.Members.Contains(memberEntity))
            {
                eventEntity.Members.Add(memberEntity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }


        public async Task<List<MemberEntity>> Get(Guid eventId)
        {
            var memberEntities = await _context.Events
                .AsNoTracking()  
                .Where(e => e.Id == eventId)
                .SelectMany(e => e.Members) 
                .ToListAsync();

            return memberEntities;
        }

        public async Task<MemberEntity?> GetById(Guid id)
        {
            var memberEntity = await _context.Members
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return memberEntity;
        }


        public async Task<bool> Delete(Guid eventId, Guid memberId)
        {
           
            var eventEntity = await _context.Events
                .Include(e => e.Members)
                .FirstAsync(e => e.Id == eventId);

            var memberEntity = eventEntity.Members
                .FirstOrDefault(m => m.Id == memberId);

            if (memberEntity == null)
            {
                return false; 
            }

            eventEntity.Members.Remove(memberEntity);
            await _context.SaveChangesAsync();

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
