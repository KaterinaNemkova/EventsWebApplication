using EventsWebApplication.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Tests.EventsRepository
{
    public class RepositoryTestsBase
    {
        protected EventsApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<EventsApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var mockAuthOptions = new Mock<IOptions<AuthorizationOptions>>();
            mockAuthOptions.Setup(o => o.Value).Returns(new AuthorizationOptions());

            var dbContext = new EventsApplicationDbContext(options, mockAuthOptions.Object);

            return dbContext;
        }
    }
}
