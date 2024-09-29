using EventsWebApplication.Core.Entities;
using EventsWebApplication.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace EventsWebApplication.DataAccess
{
    public class EventsApplicationDbContext:DbContext
    {
        private readonly AuthorizationOptions _authOptions;

        public EventsApplicationDbContext(DbContextOptions<EventsApplicationDbContext> options, IOptions<AuthorizationOptions> authOptions)
            : base(options)
        {
            _authOptions = authOptions.Value;
        }

        public DbSet<EventEntity> Events { get; set; }

        public DbSet<MemberEntity> Members { get; set; }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<RoleEntity> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventEntity>()
                .Property(e => e.EventCategory)
                .HasConversion<string>();  // Преобразование enum в строку
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventsApplicationDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(_authOptions));
        }

    }
}
