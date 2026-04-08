using Core.Persistence.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class UserRepository : EFEntityBaseRepository<User, ShelfManagerDbContext>, IUserRepository
{
    public UserRepository(ShelfManagerDbContext context) : base(context)
    {
    }
}
