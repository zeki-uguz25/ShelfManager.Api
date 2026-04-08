using Core.Persistence.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class UserBookRepository : EFEntityBaseRepository<UserBook, ShelfManagerDbContext>, IUserBookRepository
{
    public UserBookRepository(ShelfManagerDbContext context) : base(context)
    {
    }
}
