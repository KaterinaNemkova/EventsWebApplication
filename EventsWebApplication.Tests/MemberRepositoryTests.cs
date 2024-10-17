using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess;
using EventsWebApplication.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xunit;

namespace EventsWebApplication.Tests
{
    public class MemberRepositoryTests
    {
        private readonly EventsApplicationDbContext _context;
        private readonly MemberRepository _memberRepository;

        public MemberRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<EventsApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new EventsApplicationDbContext(options, Options.Create(new AuthorizationOptions()));
            _memberRepository = new MemberRepository(_context);
        }

        [Fact]
        public async Task Add_ShouldReturnTrue_WhenMemberAddedSuccessfully()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var surname = "Doe";
            var birthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20));
            var registrDate = DateOnly.FromDateTime(DateTime.Now);

            var eventEntity = new EventEntity { Id = eventId, Members = new List<MemberEntity>() };
            var userEntity = new UserEntity { Id = userId, UserName = "JohnDoe", Email = "johndoe@example.com" };
            
            _context.Events.Add(eventEntity);
            _context.Users.Add(userEntity);

            await _context.SaveChangesAsync();
            
            // Act
            var result = await _memberRepository.Add(eventId, userId, surname, birthDate, registrDate);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetById_ShouldReturnMember_WhenMemberExists()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            var memberEntity = new MemberEntity { Id = memberId, Name = "John", Surname = "Doe" };

            _context.Members.Add(memberEntity);
            await _context.SaveChangesAsync();

            // Act
            var result = await _memberRepository.GetById(memberId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memberId, result?.Id);
        }

        [Fact]
        public async Task GetById_ShouldReturnNull_WhenMemberDoesNotExist()
        {
            // Arrange
            var memberId = Guid.NewGuid();

            // Act
            var result = await _memberRepository.GetById(memberId);

            // Assert
            Assert.Null(result);
        }
    }

}
