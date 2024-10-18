using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Events.UseCases.GetEventByName;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.EventsUseCases
{
    public class GetEventByNameTest
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidationService> _validationServiceMock;
        private readonly GetEventByNameUseCase _getEventByNameUseCase;

        public GetEventByNameTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _validationServiceMock = new Mock<IValidationService>();

            _unitOfWorkMock.Setup(u => u.eventRepository).Returns(_eventRepositoryMock.Object);

            _getEventByNameUseCase = new GetEventByNameUseCase(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _validationServiceMock.Object
            );
        }

        [Fact]
        public async Task GetByName_ShouldReturnEventDto_WhenEventExists()
        {
            var request = new GetEventByNameRequest { Name = "Test Event" };
            var eventEntity = new EventEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Description = "Test Description"
            };

            var eventDto = new EventDto
            {
                Id = eventEntity.Id,
                Title = eventEntity.Title,
                Description = eventEntity.Description
            };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(r => r.GetByNameAsync(request.Name)).ReturnsAsync(eventEntity);
            _mapperMock.Setup(m => m.Map<EventDto>(eventEntity)).Returns(eventDto);

            var result = await _getEventByNameUseCase.GetByName(request);

            Assert.NotNull(result);
            Assert.Equal(eventDto.Id, result.Id);
            Assert.Equal(eventDto.Title, result.Title);
            Assert.Equal(eventDto.Description, result.Description);

            _validationServiceMock.Verify(v => v.ValidateAsync(request), Times.Once);
            _eventRepositoryMock.Verify(r => r.GetByNameAsync(request.Name), Times.Once);
            _mapperMock.Verify(m => m.Map<EventDto>(eventEntity), Times.Once);
        }

        [Fact]
        public async Task GetByName_ShouldThrowKeyNotFoundException_WhenEventNotFound()
        {
            var request = new GetEventByNameRequest { Name = "Non-Existent Event" };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(r => r.GetByNameAsync(request.Name)).ReturnsAsync((EventEntity?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _getEventByNameUseCase.GetByName(request));

            _validationServiceMock.Verify(v => v.ValidateAsync(request), Times.Once);
            _eventRepositoryMock.Verify(r => r.GetByNameAsync(request.Name), Times.Once);

        }
    }
}
