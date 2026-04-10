using Core.Persistence.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShelfManager.Application.Abstractions.Repositories;
using ShelfManager.Domain.Entities;
using ShelfManager.Persistence.Context;

namespace ShelfManager.Persistence.Repositories;

public class FineRepository : EFEntityBaseRepository<Fine, ShelfManagerDbContext>, IFineRepository
{
    public FineRepository(ShelfManagerDbContext context) : base(context)
    {
    }
    //UserId veildi�inde �denmemi� cezalar�n� listeleyen metot
    public async Task<IEnumerable<Fine?>> GetUnpaidFinesByUserIdAsync(Guid userId)
    {
        return await _context.Fines
            .Where(x => x.UserId == userId && x.IsPaid == false)
            .ToListAsync();
    }

    public async Task<IEnumerable<Fine>> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Fines
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}
