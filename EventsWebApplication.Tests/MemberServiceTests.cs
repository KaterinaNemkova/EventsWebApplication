using Moq;
using AutoMapper;
using Xunit;
using EventsWebApplication.Application.Services;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;


namespace EventsWebApplication.Tests
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberRepository> _memberRepositoryMock;
        private readonly Mock<IEventService> _eventServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly MemberService _memberService;

        public MemberServiceTests()
        {
            _memberRepositoryMock = new Mock<IMemberRepository>();
            _eventServiceMock = new Mock<IEventService>();
            _mapperMock = new Mock<IMapper>();
            _userServiceMock = new Mock<IUserService>();
            _memberService = new MemberService(
                _memberRepositoryMock.Object,
                _eventServiceMock.Object,
                _mapperMock.Object,
                _userServiceMock.Object
            );
        }

        [Fact]
        public async Task AddToEvent_ShouldReturnEventNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            string surname = "Test";
            var birthDate = new DateOnly(2000, 1, 1);
            var registrDate = new DateOnly(2024, 1, 1);

            _eventServiceMock.Setup(es => es.Exist(eventId)).ReturnsAsync(false);

            // Act
            var result = await _memberService.AddToEvent(eventId, userId, surname, birthDate, registrDate);

            // Assert
            Assert.Equal(AddToEventResult.EventNotFound, result);
        }

        [Fact]
        public async Task AddToEvent_ShouldReturnUserNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            string surname = "Test";
            var birthDate = new DateOnly(2000, 1, 1);
            var registrDate = new DateOnly(2024, 1, 1);

            _eventServiceMock.Setup(es => es.Exist(eventId)).ReturnsAsync(true);
            _userServiceMock.Setup(us => us.AlreadyExist(userId)).ReturnsAsync(false);

            // Act
            var result = await _memberService.AddToEvent(eventId, userId, surname, birthDate, registrDate);

            // Assert
            Assert.Equal(AddToEventResult.UserNotFound, result);
        }

        [Fact]
        public async Task AddToEvent_ShouldReturnAlreadyExists_WhenUserIsAlreadyInEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            string surname = "Test";
            var birthDate = new DateOnly(2000, 1, 1);
            var registrDate = new DateOnly(2024, 1, 1);

            _eventServiceMock.Setup(es => es.Exist(eventId)).ReturnsAsync(true);
            _userServiceMock.Setup(us => us.AlreadyExist(userId)).ReturnsAsync(true);
            _memberRepositoryMock.Setup(mr => mr.IsMemberInEvent(eventId, userId)).ReturnsAsync(true);

            // Act
            var result = await _memberService.AddToEvent(eventId, userId, surname, birthDate, registrDate);

            // Assert
            Assert.Equal(AddToEventResult.AlreadyExists, result);
        }

        [Fact]
        public async Task AddToEvent_ShouldReturnSuccess_WhenAddingMember()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            string surname = "Test";
            var birthDate = new DateOnly(2000, 1, 1);
            var registrDate = new DateOnly(2024, 1, 1);

            _eventServiceMock.Setup(es => es.Exist(eventId)).ReturnsAsync(true);
            _userServiceMock.Setup(us => us.AlreadyExist(userId)).ReturnsAsync(true);
            _memberRepositoryMock.Setup(mr => mr.IsMemberInEvent(eventId, userId)).ReturnsAsync(false);
            _memberRepositoryMock.Setup(mr => mr.Add(eventId, userId, surname, birthDate, registrDate)).ReturnsAsync(true);

            // Act
            var result = await _memberService.AddToEvent(eventId, userId, surname, birthDate, registrDate);

            // Assert
            Assert.Equal(AddToEventResult.Success, result);
        }

        [Fact]
        public async Task Get_ShouldThrowInvalidOperationException_WhenEventNotFound()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _eventServiceMock.Setup(es => es.Exist(eventId)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memberService.Get(eventId));
        }

        [Fact]
        public async Task GetById_ShouldThrowInvalidOperationException_WhenMemberNotFound()
        {
            // Arrange
            var memberId = Guid.NewGuid();
            _memberRepositoryMock.Setup(mr => mr.GetById(memberId)).ReturnsAsync((MemberEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _memberService.GetById(memberId));
        }

        [Fact]
        public async Task DeleteMember_ShouldReturnEventNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var memberId = Guid.NewGuid();

            _eventServiceMock.Setup(es => es.Exist(eventId)).ReturnsAsync(false);

            // Act
            var result = await _memberService.DeleteMember(eventId, memberId);

            // Assert
            Assert.Equal(DeleteMemberResults.EventNotFound, result);
        }

        [Fact]
        public async Task DeleteMember_ShouldReturnMemberNotFound_WhenMemberDoesNotExist()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var memberId = Guid.NewGuid();

            _eventServiceMock.Setup(es => es.Exist(eventId)).ReturnsAsync(true);
            _memberRepositoryMock.Setup(mr => mr.AlreadyExist(memberId)).ReturnsAsync(false);

            // Act
            var result = await _memberService.DeleteMember(eventId, memberId);

            // Assert
            Assert.Equal(DeleteMemberResults.MemberNotFound, result);
        }

        [Fact]
        public async Task DeleteMember_ShouldReturnSuccess_WhenMemberIsDeleted()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var memberId = Guid.NewGuid();

            _eventServiceMock.Setup(es => es.Exist(eventId)).ReturnsAsync(true);
            _memberRepositoryMock.Setup(mr => mr.AlreadyExist(memberId)).ReturnsAsync(true);
            _memberRepositoryMock.Setup(mr => mr.Delete(eventId, memberId)).ReturnsAsync(true);

            // Act
            var result = await _memberService.DeleteMember(eventId, memberId);

            // Assert
            Assert.Equal(DeleteMemberResults.Success, result);
        }
    }
}
