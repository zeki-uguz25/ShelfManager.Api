using Core.Persistence.EntityFrameworkCore.BaseRepositories;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Abstractions.Repositories;

public interface IFineRepository : IRepository<Fine>
{
    Task<IEnumerable<Fine?>> GetUnpaidFinesByUserIdAsync(Guid userId);
    Task<IEnumerable<Fine>> GetAllByUserIdAsync(Guid userId);
}
