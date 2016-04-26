using Code9.net._2016.data.Entities;
using Code9.net._2016.Repositories;
using Code9.net._2016.Repositories.Models;
using Code9.net._2016.Web.Models.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Code9.net._2016.Web.Controllers
{
    public class OrdersController : ApiController
    {
        private IRestourantRepository repository { get; set; }

        public OrdersController(IRestourantRepository repository)
        {
            this.repository = repository;
        }
        // GET api/<controller>?table=<tablenum> or GET api/<controller>?kind=0|1 - table filter has precedence
        public IEnumerable<Order> Get(OrderFilter filter)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            if (filter.Table != null && filter.Table.HasValue)
            {
                var orders = repository.GetOpenOrdersForTable(filter.Table.Value);
                var ret = new List<Order>();
                foreach (var item in orders)
                {
                    var order = new Order()
                    {
                        Id = item.ID,
                        MenuId = item.Item.ID,
                        Quantity = item.Quantity,
                        Table = item.Table,
                        Delivered = item.Delivered,
                        Payed = item.Payed
                    };
                    ret.Add(order);
                }
                return ret;
            }
            else if (filter.Kind !=null && filter.Kind.HasValue)
            {
                if (!Enum.IsDefined(typeof(MenuItemKind), filter.Kind.Value))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }
                var orders = repository.GetOpenOrdersForKind(filter.Kind.Value);
                return orders.Select(item => new Order()
                {
                    Id = item.ID,
                    MenuId = item.Item.ID,
                    Quantity = item.Quantity,
                    Table = item.Table,
                    Delivered = item.Delivered,
                    Payed = item.Payed
                });
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        // GET api/<controller>/5
        public Order Get(int id)
        {
            var order = repository.GetOrderByID(id);
            if (order == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return new Order()
            {
                Id = order.ID,
                MenuId = order.Item.ID,
                Quantity = order.Quantity,
                Table = order.Table,
                Delivered = order.Delivered,
                Payed = order.Payed
            };
        }

        // POST api/<controller>/5 <- table num
        public void Post(int id, [FromBody]Order value)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var order = new SubmittedOrderItem()
            {
                MenuItemID = value.MenuId,
                Quantity = value.Quantity
            };
            repository.AddOrdersForTableByWorker(new[] { order }, id, EmployeeRole.BARTENDER);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]Order value)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            var menuItem = repository.GetMenuItemByID(value.MenuId);
            if (menuItem == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            var order = new OrderItem()
            {
                ID = value.Id,
                Delivered = value.Delivered,
                Item = menuItem,
                Payed = value.Payed,
                Quantity = value.Quantity,
                Table = value.Table
            };
            repository.UpdateOrderItem(order);
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            repository.DeleteOrder(id, EmployeeRole.BARTENDER);
        }
    }
}