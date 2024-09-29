using EventsWebApplication.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Core.Enums;

namespace EventsWebApplication.DataAccess.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.HasKey(x => x.Id);

            var permissions = Enum
                .GetValues<Permission>()
                .Select(r => new PermissionEntity
                {
                    Id = (int)r,
                    Name = r.ToString()
                });

            builder.HasData(permissions);
        }
    }
}
