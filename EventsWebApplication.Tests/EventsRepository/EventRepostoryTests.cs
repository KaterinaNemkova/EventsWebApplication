using EventsWebApplication.Core.Entities;
using EventsWebApplication.Core.Enums;
using EventsWebApplication.DataAccess.Repositories;
using FluentAssertions;
using Xunit;

namespace EventsWebApplication.Tests.EventsRepository
{
    public class EventRepositoryTests : RepositoryTestsBase
    {
        [Fact]
        public async Task GetByNameAsync_ShouldReturnEvent_WhenEventExists()
        {
            var dbContext = GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var eventEntity = new EventEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Description = "Test Description",
                DateTime = DateTime.Now,
                Place = "Test Place",
                EventCategory = EventsCategory.Conference,
                MaxCountPeople = 100
            };
            dbContext.Events.Add(eventEntity);
            await dbContext.SaveChangesAsync();

            var result = await repository.GetByNameAsync("Test Event");

            result.Should().NotBeNull();
            result!.Title.Should().Be("Test Event");
        }

        [Fact]
        public async Task GetByNameAsync_ShouldReturnNull_WhenEventDoesNotExist()
        {
            var dbContext = GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);

            var result = await repository.GetByNameAsync("Non-Existent Event");

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddEventEntity_WhenValidEntityIsProvided()
        {
            var newEvent = new EventEntity
            {
                Id = Guid.NewGuid(),
                Title = "Test Event",
                Description = "Test Description",
                DateTime = DateTime.UtcNow,
                Place = "Test Place",
                EventCategory = EventsCategory.Conference,
                MaxCountPeople = 100
            };

            var dbContext = GetInMemoryDbContext();
            var repository = new EventRepository(dbContext);
            
            await repository.CreateAsync(newEvent);
            await dbContext.SaveChangesAsync();

            var eventInDb = await dbContext.Events.FindAsync(newEvent.Id);
            Assert.NotNull(eventInDb); 
            Assert.Equal(newEvent.Title, eventInDb.Title);
            Assert.Equal(newEvent.Description, eventInDb.Description);
        }
    }
}
