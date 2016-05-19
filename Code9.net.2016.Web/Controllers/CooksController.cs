using Code9.net._2016.Repositories;
using Code9.net._2016.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Code9.net._2016.Web.Controllers
{
    public class CooksController : Controller
    {
        public IRestourantRepository repository { get; set; }

        public CooksController(IRestourantRepository repository)
        {
            this.repository = repository;
        }
        // GET: Cooks
        public ActionResult Index()
        {
            var todoMeals = repository.GetOpenOrdersForKind(data.Entities.MenuItemKind.MEAL);
            var model = new List<Order>();
            foreach (var item in todoMeals)
            {
                model.Add(new Order()
                {
                    ID = item.ID,
                    Delivered = item.Delivered,
                    DisplayName = item.Item.DisplayName,
                    Price = item.Item.Price,
                    Quantity = item.Quantity
                });
            }
            return View(model.ToArray());
        }

        public ActionResult FulfillOrder(int id)
        {
            try
            {
                repository.FulfillOrder(id, data.Entities.EmployeeRole.COOK);
                return RedirectToAction("Index");
            }
            catch (ArgumentException ex)
            {
                return View("Error");
            }
        }
    }
}