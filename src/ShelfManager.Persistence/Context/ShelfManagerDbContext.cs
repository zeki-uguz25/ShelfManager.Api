using Microsoft.EntityFrameworkCore;
using ShelfManager.Domain.Common;
using ShelfManager.Domain.Entities;
using System.Linq.Expressions;

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

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(AuditableEntity.IsDeleted));
                    var filter = Expression.Lambda(Expression.Not(property), parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}
