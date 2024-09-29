using EventsWebApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Core.Configurations
{
    public class EventConfiguration:IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder) 
        { 
            builder.HasKey(e => e.Id);

            builder.HasMany(x => x.Members)
                .WithMany(x => x.Events);
        }

    }
}
