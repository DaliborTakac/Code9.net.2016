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
        IEnumerable<Employee> GetAllWorkersForRole(EmployeeRole role);
        IEnumerable<OrderItem> GetOpenOrdersForTable(int table);
        IEnumerable<OrderItem> GetOpenOrdersForKind(MenuItemKind kind);
        IEnumerable<MenuItem> GetMenuForItemKind(MenuItemKind kind);
        void AddMenuItemToMenu(string name, double price, MenuItemKind kind, Employee worker);
        void AddOrderForTableByWorker(MenuItem item, int quantity, int table, Employee worker);
        void FulfillOrder(OrderItem order, Employee worker);
        void CheckoutTable(int table, Employee worker);
    }
}
