using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebApi.Models;

namespace WebApi
{
    public class CustomersRepository : ICustomersRepository
    {
        private CustomerDbContext _customerContext;


        public CustomersRepository(CustomerDbContext customerContext)
        {
            _customerContext = customerContext;
        }

        public async Task<long> Create(Customer NewCustomer)
        {
            if (FindCustomerId(NewCustomer.lastname, NewCustomer.firstname).GetAwaiter().GetResult() > 0)
            {
                return 0;
            }
            else
            {
                await _customerContext.customers.AddAsync(NewCustomer);
                int ins = _customerContext.SaveChangesAsync().GetAwaiter().GetResult();
                return FindCustomerId(NewCustomer.lastname, NewCustomer.firstname).Result;
            }
            
        }

        public async Task<Customer> Get(long CustomerId)
        {
            var customerExists = _customerContext.customers.Any(i => i.id == CustomerId);
            return customerExists ? await _customerContext.customers.FindAsync(CustomerId) : null;
        }

        public async Task<long> FindCustomerId(string Lname, string Fname)
        {
            var c = await _customerContext.customers.Where(i => i.lastname == Lname).Where(j => j.firstname == Fname).FirstOrDefaultAsync();
            return c != null ? c.id : -1;
        }
    }
}
