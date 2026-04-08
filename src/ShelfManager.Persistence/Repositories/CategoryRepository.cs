using Core.Persistence.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class CategoryRepository : EFEntityBaseRepository<Category, ShelfManagerDbContext>, ICategoryRepository
{
    public CategoryRepository(ShelfManagerDbContext context) : base(context)
    {
    }
}
