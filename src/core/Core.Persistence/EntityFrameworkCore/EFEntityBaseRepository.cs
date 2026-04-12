using Core.Persistence.EntityFrameworkCore.BaseRepositories;
using Core.Persistence.EntityFrameworkCore.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.EntityFrameworkCore;

public class EFEntityBaseRepository<T, TContext> : IRepository<T>//Amacı bu kodları her repo da yazmak yerine
    where T : class//bir kere burda yazdık sonra burdan kalıtım verdik
    where TContext : DbContext//BookRepository : EFEntityBaseRepository<Book,
                              //ShelfManagerDbContext> diyince T=Book, TContext=ShelfManagerDbContext
{
    protected readonly TContext _context;//repolar veritabanı ile ilgili işlemleri yürütür.

    public EFEntityBaseRepository(TContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
    public async Task<PagedList<T>> GetPagedAsync(int pageNumber, int pageSize)
    {
        var totalCount = await _context.Set<T>().CountAsync();
        var items = await _context.Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return new PagedList<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
