using Core.Persistence.EntityFrameworkCore.BaseRepositories;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Abstractions.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name);
    Task<Role?> GetByCodeAsync(string code);
    Task<IEnumerable<string>> GetPermissionsByRoleIdAsync(Guid roleId);
}
