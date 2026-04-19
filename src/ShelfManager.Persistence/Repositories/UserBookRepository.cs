using Core.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class UserBookRepository : EFEntityBaseRepository<UserBook, ShelfManagerDbContext>, IUserBookRepository
{
    public UserBookRepository(ShelfManagerDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<UserBook>> GetBorrowedBookCountByUserIdAsync(Guid userId)
    {
        return await _context.UserBooks
            .Include(x => x.Book)
            .Where(x=> x.UserId == userId&& x.IsReturned==false)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserBook>> GetAllBookByUserIdAsync(Guid userId)
    {
        return await _context.UserBooks
            .Include(x => x.Book)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    
}
