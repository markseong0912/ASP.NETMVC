﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
		// GET: Customers
		private ApplicationDbContext _context;
        public CustomersController()
        {
			_context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
			//base.Dispose(disposing);	
			_context.Dispose();
        }

		public ActionResult New() // /Movies/New
        {
			var membershipTypes = _context.MembershipTypes.ToList();
			var viewModel = new CustomerFormViewModel
			{
				MembershipTypes = membershipTypes
			};
			return View("CustomerForm", viewModel); //CustomerForm -> view name
        }
		//public ActionResult Create(NewCustomerViewModel viewModel)
  //      {
			
		//	return View();
  //      }
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public ActionResult Save(Customer customer) //saving data
        {
			if (!ModelState.IsValid)
			{
				var viewModel = new CustomerFormViewModel
				{
					Customer = customer,
					MembershipTypes = _context.MembershipTypes.ToList()
				};

				return View("CustomerForm", viewModel);
			}

			if (customer.Id == 0)
				_context.Customers.Add(customer);
			else
			{
				var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);
				customerInDb.Name = customer.Name;
				customerInDb.Birthdate = customer.Birthdate;
				customerInDb.MembershipTypeId = customer.MembershipTypeId;
				customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
			}

			_context.SaveChanges();

			return RedirectToAction("Index", "Customers");
		}
		public ViewResult Index()
		{
			var customers = _context.Customers.Include(c => c.MembershipType).ToList();

			return View(customers);
		}
		public ActionResult Details(int id)
		{
			var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);

			if (customer == null)
				return HttpNotFound();

			return View(customer);
		}
		public ActionResult Edit(int id)
        {
			var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

			if (customer == null)
            {
				return HttpNotFound();
			}

			var viewModel = new CustomerFormViewModel
			{
				Customer = customer,
				MembershipTypes = _context.MembershipTypes.ToList()
			};
			return View("CustomerForm", viewModel);
        }
		//      public ViewResult Index()
		//{
		//	var customers = GetCustomers();
		//	return View(customers);
		//}

		//public ActionResult Details(int id)
		//{
		//	var customer = GetCustomers().SingleOrDefault(c => c.Id == id);
		//	if (customer == null)
		//		return HttpNotFound();
		//	return View(customer);
		//}

		//private IEnumerable<Customer> GetCustomers()
		//{
		//	return new List<Customer>
		//	{
		//		new Customer {Id = 1, Name = "John Smith"},
		//		new Customer {Id = 2, Name = "Mary Williams"}
		//	};
		//}
	}
}