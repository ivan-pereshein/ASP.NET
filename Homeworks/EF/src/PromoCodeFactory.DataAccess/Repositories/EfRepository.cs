using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EfRepository<T> : IRepository<T>
                where T : BaseEntity
    {
        protected readonly EfContext _context;

        public EfRepository(EfContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            DbSet<T> set = _context.Set<T>();
            return await set.ToListAsync<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var query = _context.Set<T>().AsQueryable<T>();
            return await query.SingleOrDefaultAsync(item => item.Id == id);
        }

    }
}
