using EventsWebApplication.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.DataAccess.Configurations
{
    public class MemberConfiguration:IEntityTypeConfiguration<MemberEntity>
    {
        public void Configure(EntityTypeBuilder<MemberEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasMany(x => x.Events)
                .WithMany(x => x.Members);
        }
    }
}
