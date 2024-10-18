using AutoMapper;
using EventsWebApplication.Application.Events.UseCases.CreateEvent;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using Moq;
using Xunit;


namespace EventsWebApplication.Tests.UseCases
{
    public class CreateEventUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidationService> _validationServiceMock;
        private readonly CreateEventUseCase _createEventUseCase;

        public CreateEventUseCaseTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _mapperMock = new Mock<IMapper>();
            _validationServiceMock = new Mock<IValidationService>();
            _unitOfWorkMock.Setup(u => u.eventRepository).Returns(_eventRepositoryMock.Object);
            _createEventUseCase = new CreateEventUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _validationServiceMock.Object);
        }

        [Fact]
        public async Task Create_ShouldCreateEvent_WhenRequestIsValid()
        {
            var request = new CreateEventRequest
            {
               Title="Title",
               Description="Sescription",
               DateTime=DateTime.Now,
               EventsCategory=EventsCategory.Anniversary,
               MaxCountPeople=10,
               Place="Place"
            };

            var eventEntity = new EventEntity { Id = Guid.NewGuid() };

            _mapperMock.Setup(m => m.Map<EventEntity>(request)).Returns(eventEntity);

            _eventRepositoryMock.Setup(r => r.GetByIdAsync(eventEntity.Id)).ReturnsAsync((EventEntity?)null);

            _validationServiceMock.Setup(v => v.ValidateAsync(It.IsAny<CreateEventRequest>())).Returns(Task.CompletedTask);

            await _createEventUseCase.Create(request);

            _eventRepositoryMock.Verify(r => r.CreateAsync(eventEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldThrowException_WhenEventWithSameIdExists()
        {
            var request = new CreateEventRequest
            {
                Title = "Title",
                Description = "Sescription",
                DateTime = DateTime.Now,
                EventsCategory = EventsCategory.Anniversary,
                MaxCountPeople = 10,
                Place = "Place"
            };

            var eventEntity = new EventEntity { Id = Guid.NewGuid() };

            _mapperMock.Setup(m => m.Map<EventEntity>(request)).Returns(eventEntity);

            _eventRepositoryMock.Setup(r => r.GetByIdAsync(eventEntity.Id)).ReturnsAsync(eventEntity);

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _createEventUseCase.Create(request));

            Assert.Equal($"Event with id {eventEntity.Id} already exists.", exception.Message);
            _eventRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<EventEntity>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

    }
}
