using Core.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class RoleRepository : EFEntityBaseRepository<Role, ShelfManagerDbContext>, IRoleRepository
{
    public RoleRepository(ShelfManagerDbContext context) : base(context)
    {
    }
    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles.FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<Role?> GetByCodeAsync(string code)
    {
        return await _context.Roles.FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task<IEnumerable<string>> GetPermissionsByRoleIdAsync(Guid roleId)
    {
        return await _context.RolePermissions
            .Where(x => x.RoleId == roleId)
            .Include(x => x.Permission)
            .Select(x => x.Permission.Code)
            .ToListAsync();
    }
}
