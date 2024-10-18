using AutoMapper;
using EventsWebApplication.Application.Events.UseCases.UpdateEvent;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using Moq;
using FluentAssertions;
using Xunit;

namespace EventsWebApplication.Tests.EventsUseCases
{
    public class UpdateEventTest
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IValidationService> _validationServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateEventUseCase _updateEventUseCase;

        public UpdateEventTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _validationServiceMock = new Mock<IValidationService>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock.Setup(uow => uow.eventRepository).Returns(_eventRepositoryMock.Object);
            _updateEventUseCase = new UpdateEventUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _validationServiceMock.Object);
        }

        [Fact]
        public async Task Update_ShouldUpdateEvent_WhenRequestIsValid()
        {

            var eventId = Guid.NewGuid();
            var updateRequest = new UpdateEventRequest
            {
                Id = eventId,
                Title = "New Title",
                Description = "Updated Description",
                DateTime = DateTime.UtcNow,
                Place = "New Place",
                EventCategory = EventsCategory.Corporate,
                MaxCountPeople = 100
            };

            var existingEvent = new EventEntity
            {
                Id = eventId,
                Title = "Old Title",
                Description = "Old Description",
                DateTime = DateTime.UtcNow.AddDays(-1), 
                Place = "Old Place",
                EventCategory = EventsCategory.Conference,
                MaxCountPeople = 50
            };

            var mappedEventEntity = new EventEntity
            {
                Id = eventId,
                Title = "New Title",
                Description = "Updated Description",
                DateTime = DateTime.UtcNow,
                Place = "New Place",
                EventCategory = EventsCategory.Corporate,
                MaxCountPeople = 100
            };

            _validationServiceMock.Setup(v => v.ValidateAsync(updateRequest)).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<EventEntity>(updateRequest)).Returns(mappedEventEntity);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);

            await _updateEventUseCase.Update(updateRequest);

            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(mappedEventEntity), Times.Once);
           
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldThrowException_WhenEventDoesNotExist()
        {
            var eventId = Guid.NewGuid();
            var updateRequest = new UpdateEventRequest
            {
                Id = eventId,
                Title = "New Title",
                Description = "Updated Description",
                DateTime = DateTime.UtcNow,
                Place = "New Place",
                EventCategory = EventsCategory.Corporate,
                MaxCountPeople = 100
            };

            var mappedEventEntity = new EventEntity { Id = eventId };

            _validationServiceMock.Setup(v => v.ValidateAsync(updateRequest)).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<EventEntity>(updateRequest)).Returns(mappedEventEntity);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync((EventEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _updateEventUseCase.Update(updateRequest));

            exception.Message.Should().Be($"Event with id {eventId} doesn't exist.");
            _eventRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<EventEntity>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }

    }
}
