using Code9.net._2016.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.net._2016.Repositories
{
    public interface IRestourantRepository
    {
        /// <summary>
        /// Used to populate list of possible users for simple login (realy just selecting user and not real login)
        /// </summary>
        /// <param name="role">Role filter to display only employees for particular job</param>
        /// <returns>list of employees matching the role filter</returns>
        IEnumerable<Employee> GetAllWorkersForRole(EmployeeRole role);

        /// <summary>
        /// Returnes list of open orders for specified table (open meaning not payed)
        /// </summary>
        /// <param name="table">table number</param>
        /// <returns>list of open orders for a table, empty list if no open orders exists</returns>
        IEnumerable<OrderItem> GetOpenOrdersForTable(int table);

        /// <summary>
        /// Returnes open orders of specified kind (used as filter for roles other than waiter)
        /// </summary>
        /// <param name="kind">flter used to restrict open order by kind</param>
        /// <returns>list of open orders</returns>
        IEnumerable<OrderItem> GetOpenOrdersForKind(MenuItemKind kind);

        /// <summary>
        /// List menu items of specified kind
        /// </summary>
        /// <param name="kind">kind of menu item to filter by</param>
        /// <returns>list of menu items</returns>
        IEnumerable<MenuItem> GetMenuForItemKind(MenuItemKind kind);

        /// <summary>
        /// Get MenuItem by specified ID
        /// </summary>
        /// <param name="ID">filtering ID</param>
        /// <returns>found MenuItem instance if exists, null if there's no match</returns>
        MenuItem GetMenuItemByID(int ID);

        /// <summary>
        /// Modifies menu by adding new menu item
        /// </summary>
        /// <param name="name">name of item</param>
        /// <param name="price">price of one unit of item</param>
        /// <param name="kind">item kind</param>
        /// <param name="worker">person making the change</param>
        void AddMenuItemToMenu(string name, double price, MenuItemKind kind, Employee worker);

        /// <summary>
        /// Adds new order for table
        /// </summary>
        /// <param name="item">item being added</param>
        /// <param name="quantity">quantity added</param>
        /// <param name="table">table where item is being ordered</param>
        /// <param name="worker">person serving the table</param>
        void AddOrderForTableByWorker(int menuID, int quantity, int table, Employee worker);

        /// <summary>
        /// Delete specified order
        /// </summary>
        /// <param name="iD">id of order to delete</param>
        void DeleteOrder(int iD, Employee worker);

        /// <summary>
        /// Complete order
        /// </summary>
        /// <param name="order">order being completed</param>
        /// <param name="worker">worker completing order</param>
        void FulfillOrder(int ID, Employee worker);

        /// <summary>
        /// Close entire table order
        /// </summary>
        /// <param name="table">table whose order is being closed</param>
        /// <param name="worker">worker completing table order</param>
        void CheckoutTable(int table, Employee worker);
    }
}
