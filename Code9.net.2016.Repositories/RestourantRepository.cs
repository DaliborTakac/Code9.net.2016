using Code9.net._2016.data;
using Code9.net._2016.data.Entities;
using Code9.net._2016.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.net._2016.Repositories
{
    public class RestourantRepository : IRestourantRepository
    {
        protected IRestourantContext restourantContext;

        /// <summary>
        /// injection capable costructor
        /// </summary>
        /// <param name="ctx"></param>
        public RestourantRepository(IRestourantContext ctx)
        {
            restourantContext = ctx;
        }

        public IEnumerable<OrderItem> GetOpenOrdersForTable(int table)
        {
            return restourantContext.OrdersWithMenu
                .Where(order => order.Table == table && !order.Payed)
                .ToList();
        }

        public IEnumerable<OrderItem> GetOpenOrdersForKind(MenuItemKind kind)
        {
            return restourantContext.OrdersWithMenu
                .Where(order => !order.Delivered && order.Item.Kind == kind)
                .ToList();
        }

        public IEnumerable<MenuItem> GetMenuForItemKind(MenuItemKind kind)
        {
            return (from m in restourantContext.MenuItems
                   where m.Kind == kind && m.Active
                   orderby m.DisplayName
                   select m).ToList();
            //return restourantContext.MenuItems
            //    .Where(m => m.Kind == kind && m.Active)
            //    .OrderBy(m => m.DisplayName);
        }

        public MenuItem GetMenuItemByID(int ID)
        {
            return restourantContext.MenuItems
                .Find(ID);
        }

        public OrderItem GetOrderByID(int ID)
        {
            return restourantContext.OrdersWithMenu
                .SingleOrDefault(item => item.ID == ID);
        }

        public void UpdateMenuItem(MenuItem item)
        {
            var existing = restourantContext.MenuItems.Find(item.ID);
            if (existing == null)
            {
                throw new ArgumentException("Specified item is not in the system", nameof(item));
            }
            existing.DisplayName = item.DisplayName;
            existing.Kind = item.Kind;
            existing.Price = item.Price;
            existing.Active = item.Active;
            restourantContext.SaveChanges();
        }

        public void UpdateOrderItem(OrderItem item)
        {
            var existing = restourantContext.OrdersWithMenu.SingleOrDefault(o => o.ID == item.ID);
            if (existing == null)
            {
                throw new ArgumentException("Specified item in not in the system", nameof(item));
            }
            existing.Item = item.Item;
            existing.Payed = item.Payed;
            existing.Quantity = item.Quantity;
            existing.Table = item.Table;
            existing.Delivered = item.Delivered;
            restourantContext.SaveChanges();
        }

        public MenuItem AddMenuItemToMenu(string name, double price, MenuItemKind kind, EmployeeRole worker)
        {
            if (worker!=EmployeeRole.BARTENDER && worker!=EmployeeRole.COOK)
            {
                throw new ArgumentException("Specified employee can't change menu", nameof(worker));
            }
            var menuItem = new MenuItem()
            {
                DisplayName = name,
                Price = price,
                Kind = kind,
                Active = true
            };
            restourantContext.MenuItems.Add(menuItem);
            restourantContext.SaveChanges();
            return menuItem;
        }

        public void AddOrdersForTableByWorker(IEnumerable<SubmittedOrderItem> orders, int table, EmployeeRole worker)
        {
            if (worker!=EmployeeRole.WAITER && worker!=EmployeeRole.BARTENDER)
            {
                throw new ArgumentException("Specified employee can't add orders", nameof(worker));
            }
            foreach (var item in orders)
            {
                var menu = restourantContext.MenuItems.Find(item.MenuItemID);
                if (menu == null)
                {
                    throw new ArgumentException("Specified menu item does not exist id = " + item.MenuItemID);
                }

                var orderItem = new OrderItem()
                {
                    Item = menu,
                    Quantity = item.Quantity,
                    Table = table
                };
                restourantContext.Orders.Add(orderItem);
            }
            restourantContext.SaveChanges();
        }

        public void FulfillOrder(int orderID, EmployeeRole worker)
        {
            if (worker==EmployeeRole.WAITER)
            {
                throw new ArgumentException("Specified employee can't fulfill orders", nameof(worker));
            }
            var order = restourantContext.Orders.Find(orderID);
            if (order == null)
            {
                throw new ArgumentException("Specified order does not exist", nameof(orderID));
            }
            if (order.Delivered)
            {
                throw new ArgumentException("Order is already filfilled", nameof(order));
            }
            order.Delivered = true;
            restourantContext.SaveChanges();
        }

        public void DeleteOrder(int orderID, EmployeeRole worker)
        {
            var order = restourantContext.Orders.Find(orderID);
            if (order == null)
            {
                throw new ArgumentException("Specified order does not exist", nameof(orderID));
            }
            if (order.Delivered)
            {
                throw new InvalidOperationException("Order is fulfilled and can't be deleted");
            }
            order.Quantity = 0;
            order.Delivered = true;
            restourantContext.SaveChanges();
        }

        public void CheckoutTable(int table, EmployeeRole worker)
        {
            if (worker != EmployeeRole.BARTENDER && worker!=EmployeeRole.WAITER)
            {
                throw new ArgumentException("Specified employee can't close table orders", nameof(worker));
            }
            var orders = restourantContext.Orders
                .Where(order => order.Table == table && !order.Payed).ToList();
            if (orders.Count==0)
            {
                throw new InvalidOperationException("No orders for specified table");
            }
            foreach (var order in orders)
            {
                order.Payed = true;
            }
            restourantContext.SaveChanges();
        }
    }
}
