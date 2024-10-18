using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Events.Services.GetAllEvents;
using EventsWebApplication.Application.Events.UseCases.GetAllEvents;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using FluentValidation;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.EventsUseCases
{
    public class GetAllEventsTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidationService> _validationServiceMock;
        private readonly GetAllEventsUseCase _getAllEventsUseCase;

        public GetAllEventsTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _mapperMock = new Mock<IMapper>();
            _validationServiceMock = new Mock<IValidationService>();
            _unitOfWorkMock.Setup(u => u.eventRepository).Returns(_eventRepositoryMock.Object);
            _getAllEventsUseCase = new GetAllEventsUseCase(_mapperMock.Object, _unitOfWorkMock.Object, _validationServiceMock.Object);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnEventDto_WhenRequestIsValid()
        {
            var request = new GetAllEventsRequest { PageNumber = 1, PageSize = 10 };
            var eventEntities = new List<EventEntity>
        {
            new EventEntity { Id = Guid.NewGuid(), Title = "Event 1" },
            new EventEntity { Id = Guid.NewGuid(), Title = "Event 2" }
            
        };
            var eventDtos = new List<EventDto>
        {
            new EventDto { Id = eventEntities[0].Id, Title = eventEntities[0].Title },
            new EventDto { Id = eventEntities[1].Id, Title = eventEntities[1].Title }
        };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(eventEntities);

            var result = await _getAllEventsUseCase.GetAll(request);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAll_ShouldThrowValidationException_WhenRequestIsInvalid()
        {
            var request = new GetAllEventsRequest { PageNumber = 1, PageSize = 0 };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).ThrowsAsync(new ValidationException("Invalid request."));

            await Assert.ThrowsAsync<ValidationException>(() => _getAllEventsUseCase.GetAll(request));
        }
    }
}
