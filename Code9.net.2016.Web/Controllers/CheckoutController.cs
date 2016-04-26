using Code9.net._2016.Repositories;
using Code9.net._2016.Web.Models;
using Code9.net._2016.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Code9.net._2016.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private IRestourantRepository repository { get; set; }

        public CheckoutController(IRestourantRepository repository)
        {
            this.repository = repository;
        }
        // GET: Checkout/{ID} <- table id
        public ActionResult Index(int ID)
        {
            var model = new CheckoutModel();
            model.Table = ID;
            model.Orders = repository.GetOpenOrdersForTable(ID);
            model.Total = model.Orders.Select(o => o.Quantity * o.Item.Price).Sum();
            return View(model);
        }

        [HttpPost]
        public ActionResult Checkout(int ID)
        {
            repository.CheckoutTable(ID, data.Entities.EmployeeRole.WAITER);
            return RedirectToAction("Index", "Waiters");
        }
    }
}