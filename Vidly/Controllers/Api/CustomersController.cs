using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.DTOs;
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
        public IEnumerable<CustomerDto> GetCustomers()
        {

            return _context.Customers.ToList().Select(Mapper.Map<Customer,CustomerDto>);
        }

        // GET /api/customers/1
        public IHttpActionResult GetCustomers(int i)
        {
            var customer = _context.Customers.SingleOrDefault(m => m.Id == i);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Customer,CustomerDto>(customer));
        }

        //POST /api/customer
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerdto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = Mapper.Map<CustomerDto, Customer>(customerdto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerdto.Id = customer.Id;

            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerdto);
        }

        // PUT api/customer/1
        [HttpPut]
        public void UpdateCustomer(int id, CustomerDto customerdto)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var customerindb = _context.Customers.SingleOrDefault(m => m.Id == id);
            if (customerindb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            customerindb = Mapper.Map(customerdto, customerindb);
            _context.SaveChanges();

        }

        // DELETE api/customer/1
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var customerindb = _context.Customers.SingleOrDefault(m => m.Id == id);
            if (customerindb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _context.Customers.Remove(customerindb);
            _context.SaveChanges();
        }
    }
}
