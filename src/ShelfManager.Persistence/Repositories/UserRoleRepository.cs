using Core.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class UserRoleRepository : EFEntityBaseRepository<UserRole, ShelfManagerDbContext>, IUserRoleRepository
{
    public UserRoleRepository(ShelfManagerDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId)
    {
        return await _context.UserRoles
            .Include(x => x.Role)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

}
