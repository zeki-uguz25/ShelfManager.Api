using Core.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class BookRepository : EFEntityBaseRepository<Book, ShelfManagerDbContext>, IBookRepository
{
    public BookRepository(ShelfManagerDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Book>> GetBooksByCategory(Guid categoryId)
    {
        return await _context.Books
            .Where(book => book.CategoryId == categoryId)
            .AsNoTracking()//okuma iþlemlerinde verilerin takip edilmesine gerek yok.
            .ToListAsync();
    }
}
