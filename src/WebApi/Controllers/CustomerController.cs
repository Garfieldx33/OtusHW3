using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("customers")]
    public class CustomerController : Controller
    {
        ICustomersRepository customersRepository;
        
        public CustomerController(ICustomersRepository repository) 
        {
            customersRepository = repository;
        }
        
        [HttpGet("{id:long}")]   
        public IActionResult GetCustomer([FromRoute] long id)
        {
             var customer = customersRepository.Get(id).GetAwaiter().GetResult();
            return customer != null ? Ok(customer) : NotFound();
        }


        [HttpPost("")]   
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            long i = customersRepository.Create(customer).GetAwaiter().GetResult();
            return i != 0 ? Ok(i) : Conflict();
        }
    }
}