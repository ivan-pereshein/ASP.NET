using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task AddAsync(Customer customer);

        Task DeleteAsync(Guid id);

        Task UpdateAsync(Customer customer);

        Task<bool> Exists(Guid id);

        Task<Customer> GetByIdIncludePreferencesAsync(Guid id);


    }
}
