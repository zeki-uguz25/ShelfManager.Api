using Core.Persistence.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class UserRoleRepository : EFEntityBaseRepository<UserRole, ShelfManagerDbContext>, IUserRoleRepository
{
    public UserRoleRepository(ShelfManagerDbContext context) : base(context)
    {
    }
}
