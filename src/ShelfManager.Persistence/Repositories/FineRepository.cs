using Core.Persistence.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class FineRepository : EFEntityBaseRepository<Fine, ShelfManagerDbContext>, IFineRepository
{
    public FineRepository(ShelfManagerDbContext context) : base(context)
    {
    }
}
