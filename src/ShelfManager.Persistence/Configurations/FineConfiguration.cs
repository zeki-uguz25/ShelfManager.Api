using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelfManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Persistence.Configurations
{
    public class FineConfiguration : IEntityTypeConfiguration<Fine>
    {
        public void Configure(EntityTypeBuilder<Fine> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Fines)
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x=>x.UserBook)
                .WithOne(x=>x.Fine)
                .HasForeignKey<Fine>(x => x.UserBookId);

        }
    }
}
