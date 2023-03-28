using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi
{
    public interface ICustomersRepository
    {
        Task<Customer> Get(long CustomerId);
        Task<long> Create(Customer NewCustomer);
    }
}
