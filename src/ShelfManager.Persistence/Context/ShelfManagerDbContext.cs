using Microsoft.EntityFrameworkCore;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShelfManager.Persistence.Context
{
    public class ShelfManagerDbContext : DbContext
    {
        public ShelfManagerDbContext(DbContextOptions<ShelfManagerDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Fine> Fines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Configuration
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            //modelBuilder.ApplyConfiguration(new FineConfiguration());
            //modelBuilder.ApplyConfiguration(new BookConfiguration());
            //modelBuilder.ApplyConfiguration(new UserBookConfiguration());
            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
            //modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            //modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            #endregion

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShelfManagerDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
