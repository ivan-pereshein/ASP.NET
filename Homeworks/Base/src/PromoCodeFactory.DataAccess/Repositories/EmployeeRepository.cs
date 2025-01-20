using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EmployeeRepository: InMemoryRepository<Employee>
    {
        private IList<Employee> EmployeeList => (IList<Employee>)Data;

        public EmployeeRepository(IEnumerable<Employee> data) : base(new List<Employee>(data))
        {
        }

        private int GetIndex(Guid id)
        {
            for (int i = 0; i < EmployeeList.Count; i++)
            {
                if (EmployeeList[i].Id == id)
                    return i;
            }

            return InvalidIndex;
        }

        public override Task AddAsync(Employee item)
        {
            if (GetIndex(item.Id) != InvalidIndex)
                return Task.FromException(new InvalidOperationException("duplicate Employee item"));

            EmployeeList.Add(item);

            return Task.CompletedTask;
        }

        public override Task DeleteAsync(Guid id)
        {
            int index = GetIndex(id);

            if (index == InvalidIndex)
                return Task.FromException(new InvalidOperationException("Employee item does not exist"));

            EmployeeList.RemoveAt(index);

            return Task.CompletedTask;
        }

        public override Task UpdateAsync(Employee item)
        {
            int index = GetIndex(item.Id);

            if (index == InvalidIndex)
                return Task.FromException(new InvalidOperationException("Employee item does not exist"));

            EmployeeList[index].FirstName = item.FirstName;
            EmployeeList[index].LastName = item.LastName;
            EmployeeList[index].Email = item.Email;
            EmployeeList[index].AppliedPromocodesCount = item.AppliedPromocodesCount;

            return Task.CompletedTask;
        }

        private const int InvalidIndex = int.MinValue;
    }
}