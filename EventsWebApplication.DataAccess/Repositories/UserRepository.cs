using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace EventsWebApplication.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EventsApplicationDbContext _context;

        public UserRepository(EventsApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(User user)
        {
            var roleEntity = await _context.Roles
                .SingleOrDefaultAsync(r => r.Id == (int)Role.User)
                ?? throw new Exception();

            var userEntity = new UserEntity()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Roles = { roleEntity }

            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<UserEntity?> GetByEmail(string email)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            return userEntity;
        }

        public async Task<HashSet<Permission>> GetUserPermissions(Guid UserId)
        {
            var roles = await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .ThenInclude(u => u.Permissions)
                .Where(u => u.Id == UserId)
                .Select(u => u.Roles)
                .ToArrayAsync();

            return roles
                .SelectMany(r => r)
                .SelectMany(r => r.Permissions)
                .Select(p => (Permission)p.Id)
                .ToHashSet();
        }

        public async Task<bool> AlreadyExist(Guid userId)
        {
            var userExist = await _context.Users.AnyAsync(x => x.Id == userId);

            return userExist;

        }
    }
}
