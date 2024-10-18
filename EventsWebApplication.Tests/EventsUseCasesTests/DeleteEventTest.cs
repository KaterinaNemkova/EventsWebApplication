using EventsWebApplication.Application.Events.UseCases.DeleteEvent;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using FluentAssertions;
using Moq;

using Xunit;

namespace EventsWebApplication.Tests.EventsUseCasesTests
{
    public class DeleteEventTest
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IValidationService> _validationServiceMock;
        private readonly DeleteEventUseCase _deleteEventUseCase;

        public DeleteEventTest()
        {
           
            _eventRepositoryMock = new Mock<IEventRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _validationServiceMock = new Mock<IValidationService>();
            _unitOfWorkMock.Setup(uow => uow.eventRepository).Returns(_eventRepositoryMock.Object);
            _deleteEventUseCase = new DeleteEventUseCase(_unitOfWorkMock.Object, _validationServiceMock.Object);
        }

        [Fact]
        public async Task Delete_ShouldDeleteEvent_WhenEventExists()
        {
            var eventId = Guid.NewGuid();
            var deleteRequest = new DeleteEventRequest { Id = eventId };
            var existingEvent = new EventEntity { Id = eventId };

            _validationServiceMock.Setup(v => v.ValidateAsync(deleteRequest)).Returns(Task.CompletedTask);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync(existingEvent);

            await _deleteEventUseCase.Delete(deleteRequest);

            _eventRepositoryMock.Verify(repo => repo.Delete(existingEvent), Times.Once);
 
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldThrowException_WhenEventDoesNotExist()
        {
            var eventId = Guid.NewGuid();
            var deleteRequest = new DeleteEventRequest { Id = eventId };

            _validationServiceMock.Setup(v => v.ValidateAsync(deleteRequest)).Returns(Task.CompletedTask);

            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(eventId)).ReturnsAsync((EventEntity?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _deleteEventUseCase.Delete(deleteRequest));

            exception.Message.Should().Be("Event doesn't exist.");
            _eventRepositoryMock.Verify(repo => repo.Delete(It.IsAny<EventEntity>()), Times.Never);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }
    }
}
