using Code9.net._2016.data.Entities;
using Code9.net._2016.Repositories;
using Code9.net._2016.Repositories.Models;
using Code9.net._2016.Web.Models;
using Code9.net._2016.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Code9.net._2016.Web.Controllers
{
    public class WaitersController : Controller
    {
        private IRestourantRepository repository { get; set; }

        public WaitersController(IRestourantRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            var model = new OrdersModel();
            var drinks = repository.GetMenuForItemKind(MenuItemKind.DRINK);
            var meals = repository.GetMenuForItemKind(MenuItemKind.MEAL);

            model.Drinks = new OrderGroup();
            model.Drinks.Orders = new List<Order>();
            model.Drinks.GroupPrefix = "Drinks";
            foreach (var item in drinks)
            {
                var order = new Order();
                order.DisplayName = item.DisplayName;
                order.Price = item.Price;
                order.Quantity = 0;
                order.ID = item.ID;
                model.Drinks.Orders.Add(order);
            }

            model.Meals = new OrderGroup();
            model.Meals.Orders = new List<Order>();
            model.Meals.GroupPrefix = "Meals";
            foreach (var item in meals)
            {
                var order = new Order();
                order.DisplayName = item.DisplayName;
                order.Quantity = 0;
                order.Price = item.Price;
                order.ID = item.ID;
                model.Meals.Orders.Add(order);
            }

            var table = Session.GetActiveTable();
            if (table >= 0)
            {
                model.Table = new Table()
                {
                    DisplayName = "Table #" + table,
                    Number = table
                };
            } else
            {
                model.Table = new Table()
                {
                    DisplayName = "<Please select table first>",
                    Number = -1
                };
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitOrder(OrdersModel model)
        {
            var orders = new List<SubmittedOrderItem>();
            foreach (var item in model.Drinks.Orders)
            {
                if (item.Quantity > 0)
                {
                    orders.Add(new SubmittedOrderItem()
                    {
                        MenuItemID = item.ID,
                        Quantity = item.Quantity
                    });
                }
            }
            foreach (var item in model.Meals.Orders)
            {
                if (item.Quantity > 0)
                {
                    orders.Add(new SubmittedOrderItem()
                    {
                        MenuItemID = item.ID,
                        Quantity = item.Quantity
                    });
                }
            }
            try
            {
                repository.AddOrdersForTableByWorker(orders, model.Table.Number, EmployeeRole.WAITER);
            }
            catch (ArgumentException ex)
            {
                return View("Error");
            }
            return RedirectToAction("Index");
        }
    }
}