using Core.Persistence.EntityFrameworkCore.BaseRepositories;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence.EntityFrameworkCore;

public class EFEntityBaseRepository<T, TContext> : IRepository<T>//Amacı bu kodları her repo da yazmak yerine
    where T : class//bir kere burda yazdık sonra burdan kalıtım verdik
    where TContext : DbContext//BookRepository : EFEntityBaseRepository<Book,
                              //ShelfManagerDbContext> diyince T=Book, TContext=ShelfManagerDbContext
{
    private readonly TContext _context;//repolar veritabanı ile ilgili işlemleri yürütür.

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
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }
}
