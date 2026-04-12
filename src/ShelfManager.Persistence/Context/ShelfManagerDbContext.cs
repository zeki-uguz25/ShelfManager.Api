using Microsoft.EntityFrameworkCore;
using ShelfManager.Domain.Entities;

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
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShelfManagerDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
