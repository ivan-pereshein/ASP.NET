using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class CustomerRepository : EfRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(EfContext context) : base(context)
        {
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Customers.Where(c => c.Id == id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();

        }

        public Task<bool> Exists(Guid id)
        {
            return _context.Customers.AnyAsync(c => c.Id == id);
        }

        public async Task<Customer> GetByIdIncludePreferencesAsync(Guid id)
        {
            return await _context
                .Customers
                .Include(c => c.CustomerPreferences)
                .ThenInclude(cp => cp.Preference)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            if (customer == null) 
            {
                throw new ArgumentNullException(nameof(customer));
            }

            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Customer> x = _context.Update(customer);
            
            await _context.SaveChangesAsync();

        }
    }
}
