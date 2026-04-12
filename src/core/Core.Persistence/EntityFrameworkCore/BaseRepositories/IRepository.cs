using Core.Persistence.EntityFrameworkCore.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.EntityFrameworkCore.BaseRepositories
{
    public interface IRepository<T> where T : class // T herhangi bir entity olabilir, int bool gibi value type olamaz
    {
        Task<T?> GetByIdAsync(Guid id);           // Id ile tek kayıt getirir, bulamazsa null döner
        Task<IEnumerable<T>> GetAllAsync();        // Tüm kayıtları liste olarak getirir
        Task AddAsync(T entity);                   // Yeni kayıt ekler
        Task UpdateAsync(T entity);                // Mevcut kaydı günceller
        Task DeleteAsync(T entity);                // Kaydı siler
        Task<PagedList<T>> GetPagedAsync(int pageNumber, int pageSize);

    }

}
