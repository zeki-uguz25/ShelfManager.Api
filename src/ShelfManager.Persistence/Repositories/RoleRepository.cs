using Core.Persistence.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class RoleRepository : EFEntityBaseRepository<Role, ShelfManagerDbContext>, IRoleRepository
{
    public RoleRepository(ShelfManagerDbContext context) : base(context)
    {
    }
}
