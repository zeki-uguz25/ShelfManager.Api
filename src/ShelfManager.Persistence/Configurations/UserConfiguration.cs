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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PhoneNumber).HasMaxLength(11);

            builder.Property(x => x.PasswordHash).IsRequired();//Bunu eklemezsek veri tabanında o kolon null olabilir hale gelir.
            builder.Property(x => x.PasswordSalt).IsRequired();

            builder.HasIndex(x => x.Email).IsUnique(); // email tekrar etmesin

            builder.Property(x => x.Address).HasMaxLength(200);

            

        }
    }
}
