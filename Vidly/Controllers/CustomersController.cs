using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult New()
        {
            var membershiptypes = _context.MembershipTypes.ToList();
            var ViewModel = new CustomerFormViewModel
            {
                Customer = new Customer(),
                MembershipTypes = membershiptypes
            };
            return View("CustomerForm",ViewModel);
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(m => m.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            var viewmodel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()   
            };

            return View("CustomerForm",viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                CustomerFormViewModel customerformvm = new CustomerFormViewModel { Customer = customer, MembershipTypes = _context.MembershipTypes.ToList() };
                return View("CustomerForm", customerformvm);
            }


            if (customer.Id==0)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                var customerindb = _context.Customers.SingleOrDefault(m => m.Id == customer.Id);
                customerindb.Name = customer.Name;
                customerindb.Birthdate = customer.Birthdate;
                customerindb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
                customerindb.MembershipTypeId = customer.MembershipTypeId;


            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        

        // GET: Customers
        public ActionResult Index()
        {
            List<Customer> customerslist = _context.Customers.Include(c => c.MembershipType).ToList();
            
            return View(customerslist);
        }

        [Route("customers/detail/{id}")]
        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id ==id);



            return View("Details", customer);
        }
    }
}