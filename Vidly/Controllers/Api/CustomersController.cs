using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }
        // GET /api/customers
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        // GET /api/customers/1
        public Customer GetCustomers(int i)
        {
            var customer = _context.Customers.SingleOrDefault(m => m.Id == i);

            if (customer == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return customer;
        }

        // DELETE /api/customers/1
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(m => m.Id == id);
            if (customer == null) return;
                
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }

        // DELETE /api/customers/
        [HttpDelete]
        public void DeleteAllCustomers()
        {
            var customers = _context.Customers.ToList();
            foreach (Customer customer in customers)
            {
                _context.Customers.Remove(customer);
            }
            _context.SaveChanges();
        }
    }
}
