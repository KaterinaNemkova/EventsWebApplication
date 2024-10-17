using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.Data;


namespace EventsWebApplication.DataAccess.Repositories
{
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        public UserRepository(EventsApplicationDbContext context) : base(context) { }
        public async Task Create(UserEntity entity)
        {
            var roleEntity = await _context.Roles
                .SingleOrDefaultAsync(r => r.Id == (int)Role.Admin)
                ?? throw new Exception();

            var userEntity = new UserEntity()
            {
                Id = Guid.NewGuid(),
                Name = entity.Name,
                Surname=entity.Surname,
                BirthDate= entity.BirthDate,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                RefreshToken= entity.RefreshToken,
                RefreshTokenExpireHours= entity.RefreshTokenExpireHours,
                Roles = { roleEntity }
            };

            await _context.Users.AddAsync(userEntity);
        }

        //public async Task Update(UserEntity entity)
        //{
        //    var existingUser = await _context.Users
        //        .Include(u => u.Roles)
        //        .SingleOrDefaultAsync(u => u.Id == entity.Id)
        //        ?? throw new Exception("User not found.");

        //    existingUser.Name = entity.Name;
        //    existingUser.Surname = entity.Surname;
        //    existingUser.BirthDate = entity.BirthDate;
        //    existingUser.Email = entity.Email;
        //    existingUser.PasswordHash = entity.PasswordHash;
        //    existingUser.RefreshToken = entity.RefreshToken;
        //    existingUser.RefreshTokenExpireHours = entity.RefreshTokenExpireHours;

        //    if (entity.Roles != null && entity.Roles.Any())
        //    {
        //        existingUser.Roles.Clear();
        //        foreach (var role in entity.Roles)
        //        {
        //            existingUser.Roles.Add(role);
        //        }
        //    }
        //}

        public async Task<UserEntity?> GetByEmail(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
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

        public async Task<bool> AlreadyExist(Guid userId )
        {
            var userExist = await _context.Users.AnyAsync(x => x.Id == userId);

            return userExist;

        }
    }
}
