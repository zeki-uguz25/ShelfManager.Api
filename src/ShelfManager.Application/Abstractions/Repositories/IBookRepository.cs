using Core.Persistence.EntityFrameworkCore.BaseRepositories;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Abstractions.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetBooksByCategory(Guid categoryId);
}
