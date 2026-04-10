using Core.Persistence.EntityFrameworkCore.BaseRepositories;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Abstractions.Repositories;

public interface IUserBookRepository : IRepository<UserBook>
{
    Task<IEnumerable<UserBook>> GetBorrowedBookCountByUserIdAsync(Guid userId);
    Task<IEnumerable<UserBook>> GetAllBookByUserIdAsync(Guid userId);
}
