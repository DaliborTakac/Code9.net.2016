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

        /// <summary>
        /// Used to populate list of possible users for simple login (realy just selecting user and not real login)
        /// </summary>
        /// <param name="role">Role filter to display only employees for particular job</param>
        /// <returns>list of employees matching the role filter</returns>
        public IEnumerable<Employee> GetAllWorkersForRole(EmployeeRole role)
        {
            return restourantContext.Workers
                .Where(w => w.Role == role);
            //return from w in restourantContext.Workers
            //       where w.Role == role
            //       select w;
        }

        /// <summary>
        /// Returnes list of open orders for specified table (open meaning not payed)
        /// </summary>
        /// <param name="table">table number</param>
        /// <returns>list of open orders for a table, empty list if no open orders and bill does exists, null if no open bill exists for table</returns>
        public IEnumerable<OrderItem> GetOpenOrdersForTable(int table)
        {
            return restourantContext.Bills
                .Where(b => b.Table == table && !b.CheckedOut)
                .FirstOrDefault()
                ?.Orders;
            //return restourantContext.Bills
            //    .Where(b => b.Table == table && !b.CheckedOut)
            //    .Select(b => b.Orders)
            //    .FirstOrDefault();
        }

        /// <summary>
        /// Returnes open orders of specified kind (used as filter for roles other than waiter)
        /// </summary>
        /// <param name="kind">flter used to restrict open order by kind</param>
        /// <returns>list of open orders</returns>
        public IEnumerable<OrderItem> GetOpenOrdersForKind(MenuItemKind kind)
        {
            return restourantContext.Orders
                .Where(order => !order.Fulfilled && order.Item.Kind == kind);
        }

        /// <summary>
        /// List menu items of specified kind
        /// </summary>
        /// <param name="kind">kind of menu item to filter by</param>
        /// <returns>list of menu items</returns>
        public IEnumerable<MenuItem> GetMenuForItemKind(MenuItemKind kind)
        {
            return from m in restourantContext.MenuItems
                   where m.Kind == kind
                   orderby m.DisplayName
                   select m;
            //return restourantContext.MenuItems
            //    .Where(m => m.Kind == kind)
            //    .OrderBy(m => m.DisplayName);
        }

        /// <summary>
        /// Modifies menu by adding new menu item
        /// </summary>
        /// <param name="name">name of item</param>
        /// <param name="price">price of one unit of item</param>
        /// <param name="kind">item kind</param>
        /// <param name="worker">person making the change</param>
        public void AddMenuItemToMenu(string name, double price, MenuItemKind kind, Employee worker)
        {
            if (worker.Role!=EmployeeRole.BARTENDER && worker.Role!=EmployeeRole.COOK)
            {
                throw new ArgumentException("Specified employee can't change menu", "worker");
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

        /// <summary>
        /// Adds new order for table
        /// </summary>
        /// <param name="item">item being added</param>
        /// <param name="quantity">quantity added</param>
        /// <param name="table">table where item is being ordered</param>
        /// <param name="worker">person serving the table</param>
        public void AddOrderForTableByWorker(MenuItem item, int quantity, int table, Employee worker)
        {
            if (worker.Role!=EmployeeRole.WAITER && worker.Role!=EmployeeRole.BARTENDER)
            {
                throw new ArgumentException("Specified employee can't add orders", "worker");
            }
            var bill = restourantContext.Bills
                .Where(b => b.Table == table && !b.CheckedOut)
                .OrderByDescending(b => b.UpdatedDate)
                .FirstOrDefault();
            if (bill == null)
            {
                bill = new Bill()
                {
                    Table = table,
                    PersonServing = worker,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    Orders = new List<OrderItem>()
                };
                restourantContext.Bills.Add(bill);
            }

            var order = new OrderItem()
            {
                Item = item,
                Quantity = quantity
            };
            restourantContext.Orders.Add(order);
            bill.Orders.Add(order);
            restourantContext.SaveChanges();
        }

        /// <summary>
        /// Complete order
        /// </summary>
        /// <param name="order">order being completed</param>
        /// <param name="worker">worker completing order</param>
        public void FulfillOrder(OrderItem order, Employee worker)
        {
            if (worker.Role==EmployeeRole.WAITER)
            {
                throw new ArgumentException("Specified employee can't fulfill orders", "worker");
            }
            if (order.Fulfilled)
            {
                throw new ArgumentException("order is already filfilled", "order");
            }
            order.Fulfilled = true;
            restourantContext.SaveChanges();
        }

        /// <summary>
        /// Close entire table order
        /// </summary>
        /// <param name="table">table whose order is being closed</param>
        /// <param name="worker">worker completing table order</param>
        public void CheckoutTable(int table, Employee worker)
        {
            if (worker.Role != EmployeeRole.BARTENDER && worker.Role!=EmployeeRole.WAITER)
            {
                throw new ArgumentException("Specified employee can't close table orders", "worker");
            }
            var bill = restourantContext.Bills
                .Where(b => b.Table == table && !b.CheckedOut)
                .FirstOrDefault();

            if (bill == null || bill.Orders == null || bill.Orders.Count == 0)
            {
                throw new InvalidOperationException("table has no open orders or bills");
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
