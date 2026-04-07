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
    public class UserBookConfiguration : IEntityTypeConfiguration<UserBook>
    {
        public void Configure(EntityTypeBuilder<UserBook> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Book)
                .WithMany(x => x.UserBooks)
                .HasForeignKey(x => x.BookId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserBooks)
                .HasForeignKey(x => x.UserId);
                //.OnDelete(DeleteBehavior.Restrict); // silmeyi engelle
                //.OnDelete(DeleteBehavior.SetNull);  // null yap
                //.OnDelete(DeleteBehavior.Cascade);  // birlikte sil 


        }
    }
}