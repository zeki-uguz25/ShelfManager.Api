using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.EntityFrameworkCore.Pagination
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; } = new(); //O sayfa da dönen kayıtların listesi
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        //Önceki sayfa var mı? Sayfa 1'deysen false, 2 veya üzerindeysen true.
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
