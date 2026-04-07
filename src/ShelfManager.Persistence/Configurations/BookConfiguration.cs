using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);//Zorunlu string alanlarda IsRequired ekleriz.
            builder.Property(x => x.Author).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Publisher).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(1000);//Zorunlu olmayan alanlarda buna gerek yoktur.
            builder.Property(x => x.Language).HasMaxLength(50);
            builder.Property(x => x.CoverImageUrl).HasMaxLength(500);

            builder.HasOne(x => x.Category)//Bu entitynin bir kategorisi var
                   .WithMany(x => x.Books)//Bir kategorinin birçok book u var
                   .HasForeignKey(x => x.CategoryId);//Bağlantı CategoryId üzerinden
        }
    }
}
