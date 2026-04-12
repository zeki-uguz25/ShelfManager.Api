using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork
    where TContext : DbContext
    {
        private readonly TContext _context;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);//Save changeleri baseRepo dan kaldırıp buraya ekledikki her handle da bir defa çağırılsın
        }
    }


}
