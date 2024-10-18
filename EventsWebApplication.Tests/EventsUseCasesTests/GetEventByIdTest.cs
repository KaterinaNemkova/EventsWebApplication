using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Events.UseCases.GetEventById;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using FluentValidation;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.EventsUseCases
{
    public  class GetEventByIdTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidationService> _validationServiceMock;
        private readonly GetEventByIdUseCase _getEventByIdUseCase;

        public GetEventByIdTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _mapperMock = new Mock<IMapper>();
            _validationServiceMock = new Mock<IValidationService>();

            _unitOfWorkMock.Setup(u => u.eventRepository).Returns(_eventRepositoryMock.Object);
            _getEventByIdUseCase = new GetEventByIdUseCase(_unitOfWorkMock.Object, _mapperMock.Object, _validationServiceMock.Object);
        }

        [Fact]
        public async Task GetById_ShouldReturnEventDto_WhenRequestIsValid()
        {
            var request = new GetEventByIdRequest { Id = Guid.NewGuid() };
            var eventEntity = new EventEntity { Id = request.Id, Title = "Test Event" };
            var eventDto = new EventDto { Id = eventEntity.Id, Title = eventEntity.Title };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id)).ReturnsAsync(eventEntity);
            _mapperMock.Setup(m => m.Map<EventDto>(eventEntity)).Returns(eventDto);

            var result = await _getEventByIdUseCase.GetById(request);

            Assert.NotNull(result);
            Assert.Equal(eventDto.Id, result.Id);
            Assert.Equal(eventDto.Title, result.Title);

            _validationServiceMock.Verify(v => v.ValidateAsync(request), Times.Once);
            _eventRepositoryMock.Verify(repo => repo.GetByIdAsync(request.Id), Times.Once);
            _mapperMock.Verify(m => m.Map<EventDto>(eventEntity), Times.Once);
        }

        [Fact]
        public async Task GetById_ShouldThrowKeyNotFoundException_WhenEventDoesNotExist()
        {
            var request = new GetEventByIdRequest { Id = Guid.NewGuid() };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id)).ReturnsAsync((EventEntity?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _getEventByIdUseCase.GetById(request));
        }

        [Fact]
        public async Task GetById_ShouldThrowValidationException_WhenRequestIsInvalid()
        {
            var request = new GetEventByIdRequest { Id = Guid.NewGuid() };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).ThrowsAsync(new ValidationException("Invalid request."));

            await Assert.ThrowsAsync<ValidationException>(() => _getEventByIdUseCase.GetById(request));
        }
    }
}
