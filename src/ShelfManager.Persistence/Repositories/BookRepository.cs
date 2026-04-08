using Core.Persistence.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class BookRepository : EFEntityBaseRepository<Book, ShelfManagerDbContext>, IBookRepository
{
    public BookRepository(ShelfManagerDbContext context) : base(context)
    {
    }
}
