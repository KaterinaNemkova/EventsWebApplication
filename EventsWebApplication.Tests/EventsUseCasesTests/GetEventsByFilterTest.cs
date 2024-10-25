using AutoMapper;
using EventsWebApplication.Application.DTOs;
using EventsWebApplication.Application.Events.UseCases.GetEventByFilter;
using EventsWebApplication.Application.Events.UseCases.GetEventsByFilter;
using EventsWebApplication.Core.Abstractions;
using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.DataAccess.Repositories;
using EventsWebApplication.DataAccess.UnitOfWork;
using Moq;
using Xunit;

namespace EventsWebApplication.Tests.EventsUseCases
{
    public class GetEventsByFilterTest
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IValidationService> _validationServiceMock;
        private readonly GetEventsByFilterUseCase _getEventsByFilterUseCase;

        public GetEventsByFilterTest()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _validationServiceMock = new Mock<IValidationService>();

            _unitOfWorkMock.Setup(u => u.eventRepository).Returns(_eventRepositoryMock.Object);

            _getEventsByFilterUseCase = new GetEventsByFilterUseCase(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _validationServiceMock.Object
            );
        }

        [Fact]
        public async Task GetByFilter_ShouldReturnFilteredEvents_WhenPlaceIsProvided()
        {
            var request = new GetEventsByFilterRequest
            {
                Place = "New York",
                DateTime=null,
                EventsCategory=null,
                PageNumber = 1,
                PageSize = 10
            };

            var eventEntities = new List<EventEntity>
        {
            new EventEntity { Id = Guid.NewGuid(), Place = "New York", Title = "Event 1" },
            new EventEntity { Id = Guid.NewGuid(), Place = "Los Angeles", Title = "Event 2" },
            new EventEntity { Id = Guid.NewGuid(), Place = "New York", Title = "Event 3" }
        };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(r => r.GetAllAsync(request.PageNumber, request.PageSize)).ReturnsAsync(eventEntities);
            _mapperMock.Setup(m => m.Map<List<EventDto>>(It.IsAny<List<EventEntity>>()))
                .Returns((List<EventEntity> entities) => entities.Select(e => new EventDto { Id = e.Id, Title = e.Title }).ToList());

            var result = await _getEventsByFilterUseCase.GetByFilter(request);

            
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count); // Only events from New York should be included
            Assert.All(result.Items, item => Assert.Equal("New York", eventEntities.First(e => e.Id == item.Id).Place));

            _validationServiceMock.Verify(v => v.ValidateAsync(request), Times.Once);
            _eventRepositoryMock.Verify(r => r.GetAllAsync(request.PageNumber, request.PageSize), Times.Once);
            _mapperMock.Verify(m => m.Map<List<EventDto>>(It.IsAny<List<EventEntity>>()), Times.Once);
        }

        [Fact]
        public async Task GetByFilter_ShouldReturnFilteredEvents_WhenDateTimeIsProvided()
        {
            var request = new GetEventsByFilterRequest
            {
                DateTime = new DateTime(2024, 10, 18),
                Place="",
                EventsCategory=null,
                PageNumber = 1,
                PageSize = 10
            };

            var eventEntities = new List<EventEntity>
        {
            new EventEntity { Id = Guid.NewGuid(), DateTime = new DateTime(2024, 10, 18), Title = "Event 1" },
            new EventEntity { Id = Guid.NewGuid(), DateTime = new DateTime(2024, 10, 19), Title = "Event 2" }
        };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(r => r.GetAllAsync(request.PageNumber, request.PageSize)).ReturnsAsync(eventEntities);
            _mapperMock.Setup(m => m.Map<List<EventDto>>(It.IsAny<List<EventEntity>>()))
                .Returns((List<EventEntity> entities) => entities.Select(e => new EventDto { Id = e.Id, Title = e.Title }).ToList());

            var result = await _getEventsByFilterUseCase.GetByFilter(request);

            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(new DateTime(2024, 10, 18), eventEntities.First(e => e.Id == result.Items.First().Id).DateTime);

            _validationServiceMock.Verify(v => v.ValidateAsync(request), Times.Once);
            _eventRepositoryMock.Verify(r => r.GetAllAsync(request.PageNumber, request.PageSize), Times.Once);
            _mapperMock.Verify(m => m.Map<List<EventDto>>(It.IsAny<List<EventEntity>>()), Times.Once);
        }

        [Fact]
        public async Task GetByFilter_ShouldThrowKeyNotFoundException_WhenEventsCategoryIsProvided()
        {
            var request = new GetEventsByFilterRequest
            {
                EventsCategory=EventsCategory.Corporate,
                Place="",
                DateTime=null,
                PageNumber = 1,
                PageSize = 10
            };

            var eventEntities = new List<EventEntity>
            {
            new EventEntity { Id = Guid.NewGuid(), Place = "New York", Title = "Event 1", EventCategory=EventsCategory.Corporate },
            new EventEntity { Id = Guid.NewGuid(), Place = "Los Angeles", Title = "Event 2", EventCategory=EventsCategory.Corporate }
            };
            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(r => r.GetAllAsync(request.PageNumber, request.PageSize)).ReturnsAsync(eventEntities);
            _mapperMock.Setup(m => m.Map<List<EventDto>>(It.IsAny<List<EventEntity>>()))
                .Returns((List<EventEntity> entities) => entities.Select(e => new EventDto { Id = e.Id, Title = e.Title }).ToList());

            var result = await _getEventsByFilterUseCase.GetByFilter(request);
            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count);
            Assert.All(result.Items, item => Assert.Equal(EventsCategory.Corporate, eventEntities.First(e => e.Id == item.Id).EventCategory));

            _validationServiceMock.Verify(v => v.ValidateAsync(request), Times.Once);
            _eventRepositoryMock.Verify(r => r.GetAllAsync(request.PageNumber, request.PageSize), Times.Once);
            _mapperMock.Verify(m => m.Map<List<EventDto>>(It.IsAny<List<EventEntity>>()), Times.Once);
        }

        [Fact]
        public async Task GetByFilter_ShouldThrowKeyNotFoundException_WhenNoEventsMatchFilter()
        {
            var request = new GetEventsByFilterRequest
            {
                Place = "Gomel",
                EventsCategory= EventsCategory.Anniversary,
                DateTime=null,
                PageNumber = 1,
                PageSize = 10
            };

            var eventEntities = new List<EventEntity>
        {
            new EventEntity { Id = Guid.NewGuid(), Place = "New York", Title = "Event 1",DateTime = new DateTime(2024, 10, 18), EventCategory=EventsCategory.Corporate },
            new EventEntity { Id = Guid.NewGuid(), Place = "Los Angeles", Title = "Event 2", DateTime = new DateTime(2024, 10, 18) , EventCategory=EventsCategory.Conference}
        };

            _validationServiceMock.Setup(v => v.ValidateAsync(request)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(r => r.GetAllAsync(request.PageNumber, request.PageSize)).ReturnsAsync(eventEntities);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _getEventsByFilterUseCase.GetByFilter(request));

            _validationServiceMock.Verify(v => v.ValidateAsync(request), Times.Once);
            _eventRepositoryMock.Verify(r => r.GetAllAsync(request.PageNumber, request.PageSize), Times.Once);
        }
    }
}
