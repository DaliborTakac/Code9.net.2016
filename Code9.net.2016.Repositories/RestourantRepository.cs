using Code9.net._2016.data;
using Code9.net._2016.data.Entities;
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

        public IEnumerable<Employee> GetAllWorkersForRole(EmployeeRole role)
        {
            return restourantContext.Workers
                .Where(w => w.Role == role)
                .ToList();
            //return from w in restourantContext.Workers
            //       where w.Role == role
            //       select w;
        }

        public IEnumerable<OrderItem> GetOpenOrdersForTable(int table)
        {
            return restourantContext.BillsWithOrdersWithMenu
                .Where(b => b.Table == table && !b.CheckedOut)
                .FirstOrDefault()
                ?.Orders ?? new List<OrderItem>();
            //return restourantContext.Bills
            //    .Where(b => b.Table == table && !b.CheckedOut)
            //    .Select(b => b.Orders)
            //    .FirstOrDefault();
        }

        public IEnumerable<OrderItem> GetOpenOrdersForKind(MenuItemKind kind)
        {
            return restourantContext.Orders
                .Where(order => !order.Fulfilled && order.Item.Kind == kind)
                .ToList();
        }

        public IEnumerable<MenuItem> GetMenuForItemKind(MenuItemKind kind)
        {
            return (from m in restourantContext.MenuItems
                   where m.Kind == kind
                   orderby m.DisplayName
                   select m).ToList();
            //return restourantContext.MenuItems
            //    .Where(m => m.Kind == kind)
            //    .OrderBy(m => m.DisplayName);
        }

        public MenuItem GetMenuItemByID(int ID)
        {
            return restourantContext.MenuItems
                .Where(m => m.ID == ID)
                .FirstOrDefault();
        }

        public void AddMenuItemToMenu(string name, double price, MenuItemKind kind, Employee worker)
        {
            if (worker.Role!=EmployeeRole.BARTENDER && worker.Role!=EmployeeRole.COOK)
            {
                throw new ArgumentException("Specified employee can't change menu", nameof(worker));
            }
            var menuItem = new MenuItem()
            {
                DisplayName = name,
                Price = price,
                Kind = kind
            };
            restourantContext.MenuItems.Add(menuItem);
            restourantContext.SaveChanges();
        }

        public void AddOrderForTableByWorker(int menuID, int quantity, int table, Employee worker)
        {
            if (worker.Role!=EmployeeRole.WAITER && worker.Role!=EmployeeRole.BARTENDER)
            {
                throw new ArgumentException("Specified employee can't add orders", nameof(worker));
            }
            var menu = restourantContext.MenuItems.Find(menuID);
            if (menu == null)
            {
                throw new ArgumentException("Specified order does not exist");
            }
            var bill = restourantContext.BillsWithOrdersWithMenu
                .Where(b => b.Table == table && !b.CheckedOut)
                .OrderByDescending(b => b.UpdatedDate)
                .FirstOrDefault();
            var dbWorker = restourantContext.Workers.Find(worker.ID);
            if (bill == null)
            {
                bill = new Bill()
                {
                    Table = table,
                    PersonServing = dbWorker,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    Orders = new List<OrderItem>()
                };
                restourantContext.Bills.Add(bill);
            }

            var order = new OrderItem()
            {
                Item = menu,
                Quantity = quantity
            };
            restourantContext.Orders.Add(order);
            bill.Orders.Add(order);
            restourantContext.SaveChanges();
        }

        public void FulfillOrder(int orderID, Employee worker)
        {
            if (worker.Role==EmployeeRole.WAITER)
            {
                throw new ArgumentException("Specified employee can't fulfill orders", nameof(worker));
            }
            var order = restourantContext.Orders.Find(orderID);
            if (order == null)
            {
                throw new ArgumentException("Specified order does not exist", nameof(orderID));
            }
            if (order.Fulfilled)
            {
                throw new ArgumentException("Order is already filfilled", nameof(order));
            }
            order.Fulfilled = true;
            restourantContext.SaveChanges();
        }

        public void DeleteOrder(int orderID, Employee worker)
        {
            if (worker == null)
            {
                throw new ArgumentNullException(nameof(worker));
            }
            var order = restourantContext.Orders.Find(orderID);
            if (order == null)
            {
                throw new ArgumentException("Specified order does not exist", nameof(orderID));
            }
            if (order.Fulfilled)
            {
                throw new InvalidOperationException("Order is fulfilled and can't be deleted");
            }
            order.Quantity = 0;
            order.Fulfilled = true;
            restourantContext.SaveChanges();
        }

        public void CheckoutTable(int table, Employee worker)
        {
            if (worker.Role != EmployeeRole.BARTENDER && worker.Role!=EmployeeRole.WAITER)
            {
                throw new ArgumentException("Specified employee can't close table orders", nameof(worker));
            }
            var bill = restourantContext.BillsWithOrdersWithMenu
                .Where(b => b.Table == table && !b.CheckedOut)
                .FirstOrDefault();

            if (bill == null || bill.Orders == null || bill.Orders.Count == 0)
            {
                throw new InvalidOperationException("Table has no open orders or bills");
            }

            bill.CheckedOut = true;
            foreach (var item in bill.Orders)
            {
                if (!item.Fulfilled)
                {
                    item.Fulfilled = true;
                }
            }
            restourantContext.SaveChanges();
        }
    }
}
