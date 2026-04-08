using Core.Persistence.EntityFrameworkCore.BaseRepositories;
using ShelfManager.Domain.Entities;

namespace ShelfManager.Application.Abstractions.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
}
