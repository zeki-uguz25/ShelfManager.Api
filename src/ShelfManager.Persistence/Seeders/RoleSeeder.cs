using Microsoft.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Services;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;
using System.Text.Json;

namespace ShelfManager.Persistence.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(ShelfManagerDbContext context, IHashingService hashingService)
        {
            await SeedPermissionsAsync(context);
            await SeedRolesAndPermissionsAsync(context);
            await SeedUsersAsync(context, hashingService);
        }

        private static async Task SeedPermissionsAsync(ShelfManagerDbContext context)
        {
            var allPermissionCodes = new List<(string Code, string Name)>
            {
                ("books.read",          "Books - Read"),
                ("books.create",        "Books - Create"),
                ("books.update",        "Books - Update"),
                ("books.delete",        "Books - Delete"),
                ("categories.manage",   "Categories - Manage"),
                ("users.get",           "Users - Get"),
                ("users.ban",           "Users - Ban"),
                ("roles.manage",        "Roles - Manage"),
                ("userbooks.borrow",    "UserBooks - Borrow"),
                ("userbooks.return",    "UserBooks - Return"),
                ("fines.pay",           "Fines - Pay"),
                ("notifications.read",  "Notifications - Read"),
            };

            foreach (var (code, name) in allPermissionCodes)
            {
                var exists = await context.Permissions.AnyAsync(x => x.Code == code);
                if (!exists)
                {
                    await context.Permissions.AddAsync(new Permission
                    {
                        Id = Guid.NewGuid(),
                        Code = code,
                        Name = name
                    });
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAndPermissionsAsync(ShelfManagerDbContext context)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "roles.json");
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var rolesFromJson = JsonSerializer.Deserialize<List<SeedRoleModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (rolesFromJson == null || !rolesFromJson.Any()) return;

            foreach (var item in rolesFromJson)
            {
                var role = await context.Roles
                    .Include(x => x.RolePermissions)
                    .FirstOrDefaultAsync(x => x.Code == item.Code);

                if (role == null)
                {
                    role = new Role
                    {
                        Id = Guid.NewGuid(),
                        Code = item.Code,
                        Name = item.Name
                    };
                    await context.Roles.AddAsync(role);
                }
                else
                {
                    role.Name = item.Name;
                    context.Roles.Update(role);
                }

                if (item.Permissions != null)
                {
                    role.RolePermissions ??= new List<RolePermission>();

                    foreach (var permCode in item.Permissions)
                    {
                        var permission = await context.Permissions.FirstOrDefaultAsync(x => x.Code == permCode);
                        if (permission == null) continue;

                        if (!role.RolePermissions.Any(x => x.PermissionId == permission.Id))
                        {
                            role.RolePermissions.Add(new RolePermission
                            {
                                Id = Guid.NewGuid(),
                                RoleId = role.Id,
                                PermissionId = permission.Id
                            });
                        }
                    }

                    var permCodesToKeep = item.Permissions.ToList();
                    var allPermissions = await context.Permissions.ToListAsync();

                    var toRemove = role.RolePermissions
                        .Where(rp => !permCodesToKeep.Contains(
                            allPermissions.FirstOrDefault(p => p.Id == rp.PermissionId)?.Code ?? ""))
                        .ToList();

                    foreach (var rp in toRemove)
                        role.RolePermissions.Remove(rp);
                }
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(ShelfManagerDbContext context, IHashingService hashingService)
        {
            if (await context.Users.AnyAsync()) return;

            var path = Path.Combine(AppContext.BaseDirectory, "users.json");
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var usersFromJson = JsonSerializer.Deserialize<List<SeedUserModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (usersFromJson == null || !usersFromJson.Any()) return;

            var allRoles = await context.Roles.ToListAsync();

            foreach (var item in usersFromJson)
            {
                var passwordHash = hashingService.HashPassword(item.Password, out var passwordSalt);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = item.FullName,
                    Email = item.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    IsActive = true,
                    IsBanned = false,
                    CreatedAt = DateTime.UtcNow
                };

                var role = allRoles.FirstOrDefault(x => x.Code == item.Role);
                if (role != null)
                {
                    user.UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            Id = Guid.NewGuid(),
                            UserId = user.Id,
                            RoleId = role.Id
                        }
                    };
                }

                await context.Users.AddAsync(user);
            }

            await context.SaveChangesAsync();
        }
    }

    public class SeedRoleModel
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public ICollection<string>? Permissions { get; set; }
    }

    public class SeedUserModel
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
