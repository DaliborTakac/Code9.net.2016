using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Code9.net._2016.data.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Code9.net._2016.Repositories.TEST
{
    [TestClass]
    public class RestourantRepositoryTests
    {
        #region GetAllWorkersForRole tests
        [TestMethod]
        public void GetAllWorkersForRole_returns_no_items_if_no_workers_match()
        {
            var mockContext = new MockDBContextFactory().Create(); // empty list for all DbSets, including workers

            var repository = new RestourantRepository(mockContext);

            var waiters = repository.GetAllWorkersForRole(EmployeeRole.WAITER);
            var bartenders = repository.GetAllWorkersForRole(EmployeeRole.BARTENDER);
            var cooks = repository.GetAllWorkersForRole(EmployeeRole.COOK);

            Assert.AreEqual(0, waiters.Count());
            Assert.AreEqual(0, bartenders.Count());
            Assert.AreEqual(0, cooks.Count());
        }

        [TestMethod]
        public void GetAllWorkersForRole_returns_one_for_each_specified_role()
        {
            var workers = new List<Employee>();
            workers.Add(new Employee()
            {
                Name = "Bob",
                Role = EmployeeRole.BARTENDER,
                ID = 1
            });
            workers.Add(new Employee()
            {
                Name = "Alice",
                Role = EmployeeRole.COOK,
                ID = 2
            });
            workers.Add(new Employee()
            {
                Name = "Charlie",
                Role = EmployeeRole.WAITER,
                ID = 3
            });

            var mockContext = new MockDBContextFactory().WithBuiltinWorkers(workers).Create();

            var repository = new RestourantRepository(mockContext);

            var waiters = repository.GetAllWorkersForRole(EmployeeRole.WAITER);
            var bartenders = repository.GetAllWorkersForRole(EmployeeRole.BARTENDER);
            var cooks = repository.GetAllWorkersForRole(EmployeeRole.COOK);

            Assert.AreEqual(1, waiters.Count());
            Assert.AreEqual(1, bartenders.Count());
            Assert.AreEqual(1, cooks.Count());

            Assert.AreEqual(3, waiters.First().ID);
            Assert.AreEqual(1, bartenders.First().ID);
            Assert.AreEqual(2, cooks.First().ID);
        }
        #endregion GetAllWorkersForRole

        #region GetOpenOrdersForTable
        [TestMethod]
        public void GetOpenOrdersForTable_returns_null_when_all_bills_are_payed()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Item = null,
                Quantity = 1,
                Fulfilled = true,
            });
            orders.Add(new OrderItem()
            {
                ID = 2,
                Item = null,
                Quantity = 10,
                Fulfilled = true,
            });

            var bills = new List<Bill>();
            bills.Add(new Bill()
            {
                ID = 1,
                Table = 4,
                CheckedOut = true,
                Orders = new List<OrderItem>() { orders[0], orders[1] },
                PersonServing = null,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).WithBuiltinBills(bills).Create();

            var repository = new RestourantRepository(mockContext);

            var openOrder = repository.GetOpenOrdersForTable(4);

            Assert.IsNull(openOrder);
        }

        [TestMethod]
        public void GetOpenOrdersForTable_returns_empty_results_when_no_open_orders()
        {
            var bills = new List<Bill>();
            bills.Add(new Bill()
            {
                ID = 1,
                Table = 4,
                CheckedOut = false,
                Orders = new List<OrderItem>(),
                PersonServing = null,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });

            var mockContext = new MockDBContextFactory().WithBuiltinBills(bills).Create();

            var repository = new RestourantRepository(mockContext);

            var openOrders = repository.GetOpenOrdersForTable(4);

            Assert.AreEqual(0, openOrders.Count());
        }

        [TestMethod]
        public void GetOpenOrdersForTable_returns_one_order_when_applicable()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 5,
                Item = null,
                Quantity = 1,
                Fulfilled = true,
            });
            orders.Add(new OrderItem()
            {
                ID = 2,
                Item = null,
                Quantity = 10,
                Fulfilled = true,
            });

            var bills = new List<Bill>();
            bills.Add(new Bill()
            {
                ID = 1,
                Table = 4,
                CheckedOut = false,
                Orders = new List<OrderItem>() { orders[0] },
                PersonServing = null,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });
            bills.Add(new Bill()
            {
                ID = 2,
                Table = 2,
                CheckedOut = true,
                Orders = new List<OrderItem>() { orders[1] },
                PersonServing = null,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).WithBuiltinBills(bills).Create();

            var repository = new RestourantRepository(mockContext);

            var openOrders = repository.GetOpenOrdersForTable(4);

            Assert.AreEqual(1, openOrders.Count());
            Assert.AreEqual(5, openOrders.First().ID);
        }
        #endregion GetOpenOrdersForTable

        #region GetOpenOrdersForKind
        [TestMethod]
        public void GetOpenOrdersForKind_returns_empty_results_when_no_unfulfilled_orders_of_specified_kind()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                ID = 1,
                DisplayName = "test menu item",
                Kind = MenuItemKind.DRINK,
                Price = 10.00
            });

            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 5,
                Item = menu[0],
                Quantity = 2,
                Fulfilled = true,
                Bill = null
            });

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            var openOrders = repository.GetOpenOrdersForKind(MenuItemKind.DRINK);

            Assert.AreEqual(0, openOrders.Count());
        }

        [TestMethod]
        public void GetOpenOrdersForKind_returns_one_order_that_matches()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                ID = 1,
                DisplayName = "test menu item",
                Kind = MenuItemKind.DRINK,
                Price = 10.00
            });

            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 5,
                Item = menu[0],
                Quantity = 2,
                Fulfilled = true,
                Bill = null
            });
            orders.Add(new OrderItem()
            {
                ID = 8,
                Item = menu[0],
                Quantity = 2,
                Fulfilled = false,
                Bill = null
            });

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            var openOrders = repository.GetOpenOrdersForKind(MenuItemKind.DRINK);

            Assert.AreEqual(1, openOrders.Count());
            Assert.AreEqual(8, openOrders.First().ID);
        }
        #endregion GetOpenOrdersForKind

        #region GetMenuForItemKind
        [TestMethod]
        public void GetMenuForItemKind_returns_proper_menu_item_for_kind()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                DisplayName = "test drink",
                Kind = MenuItemKind.DRINK,
                ID = 2,
                Price = 10.00
            });
            menu.Add(new MenuItem()
            {
                DisplayName = "test meal",
                Kind = MenuItemKind.MEAL,
                ID = 5,
                Price = 25.00
            });

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).Create();

            var repository = new RestourantRepository(mockContext);

            var drink = repository.GetMenuForItemKind(MenuItemKind.DRINK);
            var meal = repository.GetMenuForItemKind(MenuItemKind.MEAL);

            Assert.AreEqual(2, drink.First().ID);
            Assert.AreEqual(5, meal.First().ID);
        }
        #endregion GetMenuForItemKind

        #region AddMenuItemToMenu
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddMenuItemToMenu_failes_if_worker_is_waiter()
        {
            var mockContext = new MockDBContextFactory().Create();

            var repository = new RestourantRepository(mockContext);

            repository.AddMenuItemToMenu("invalid operation generating item", 5.00, MenuItemKind.DRINK, new Employee() { Role = EmployeeRole.WAITER });
        }

        [TestMethod]
        public void AddMenuItemToMenu_adds_menu_item_if_worker_is_not_waiter()
        {
            var numSaveChangesCalls = 0;
            var mockContext = new MockDBContextFactory().WithSaveChangesCallback(() => ++numSaveChangesCalls).Create();

            var repository = new RestourantRepository(mockContext);

            const string validItem1 = "valid item";

            repository.AddMenuItemToMenu(validItem1, 10.00, MenuItemKind.DRINK, new Employee() { Role = EmployeeRole.BARTENDER });

            Assert.AreEqual(1, mockContext.MenuItems.Count());
            Assert.AreEqual(validItem1, mockContext.MenuItems.Last().DisplayName);
            Assert.IsTrue(0 < numSaveChangesCalls);

            numSaveChangesCalls = 0;

            const string validItem2 = "another valid item";

            repository.AddMenuItemToMenu(validItem2, 97.99, MenuItemKind.MEAL, new Employee() { Role = EmployeeRole.COOK });

            Assert.AreEqual(2, mockContext.MenuItems.Count());
            Assert.AreEqual(validItem2, mockContext.MenuItems.Last().DisplayName);
            Assert.IsTrue(0 < numSaveChangesCalls);
        }

        [TestMethod]
        public void AddMenuItem_adds_item_with_correct_contents()
        {
            var mockContext = new MockDBContextFactory().Create();

            var repository = new RestourantRepository(mockContext);

            const string itemName = "item name";
            const double itemPrice = 19.99;
            const MenuItemKind itemKind = MenuItemKind.DRINK;

            repository.AddMenuItemToMenu(itemName, itemPrice, itemKind, new Employee() { Role = EmployeeRole.BARTENDER });

            var inserted = mockContext.MenuItems.Last();

            Assert.AreEqual(itemName, inserted.DisplayName);
            Assert.AreEqual(itemPrice, inserted.Price);
            Assert.AreEqual(itemKind, inserted.Kind);
        }
        #endregion

        #region AddOrderForTableByWorker
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOrderForTableByWorker_failes_when_worker_is_cook()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                ID = 1,
                DisplayName = "menu item",
                Price = 19.99,
                Kind = MenuItemKind.DRINK
            });

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).Create();

            var repository = new RestourantRepository(mockContext);

            repository.AddOrderForTableByWorker(menu[0], 1, 1, new Employee() { Role = EmployeeRole.COOK });
        }

        [TestMethod]
        public void AddOrderForTableByWorker_creates_new_bill_if_one_does_not_exists()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                ID = 1,
                DisplayName = "menu item",
                Price = 19.95,
                Kind = MenuItemKind.MEAL
            });

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).Create();
            var repository = new RestourantRepository(mockContext);

            repository.AddOrderForTableByWorker(menu[0], 1, 1, new Employee() { Role = EmployeeRole.WAITER });

            Assert.AreEqual(1, mockContext.Bills.Count());
        }

        [TestMethod]
        public void AddOrderForTableByWorker_reuses_existing_bill_for_table_if_not_checked_out()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                ID = 1,
                DisplayName = "menu item",
                Price = 19.95,
                Kind = MenuItemKind.MEAL
            });

            var bills = new List<Bill>();
            bills.Add(new Bill()
            {
                ID = 1,
                Table = 4,
                CheckedOut = false,
                Orders = new List<OrderItem>(),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            });

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).WithBuiltinBills(bills).Create();

            var repository = new RestourantRepository(mockContext);

            var numBils = bills.Count;

            repository.AddOrderForTableByWorker(menu[0], 1, 4, new Employee() { Role = EmployeeRole.BARTENDER });

            Assert.AreEqual(numBils, bills.Count);
        }

        [TestMethod]
        public void AddOrderForTableByWorker_adds_new_bill_if_existing_does_not_maches_table()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                ID = 1,
                DisplayName = "menu item",
                Price = 19.95,
                Kind = MenuItemKind.MEAL
            });

            var bills = new List<Bill>();
            bills.Add(new Bill()
            {
                ID = 1,
                Table = 4,
                CheckedOut = false,
                Orders = new List<OrderItem>(),
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            });

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).WithBuiltinBills(bills).Create();

            var repository = new RestourantRepository(mockContext);

            var numBils = bills.Count;

            repository.AddOrderForTableByWorker(menu[0], 1, 3, new Employee() { Role = EmployeeRole.BARTENDER });

            Assert.AreNotEqual(numBils, bills.Count);
        }

        [TestMethod]
        public void AddOrderForTableByWorker_persists_data_by_calling_save_changes_on_db_context()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                ID = 1,
                DisplayName = "menu item",
                Price = 19.95,
                Kind = MenuItemKind.MEAL
            });

            var numSaveCalls = 0;

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).WithSaveChangesCallback(() => ++numSaveCalls).Create();

            var repository = new RestourantRepository(mockContext);

            repository.AddOrderForTableByWorker(menu[0], 1, 1, new Employee() { Role = EmployeeRole.WAITER });

            Assert.AreEqual(1, numSaveCalls);
        }
        #endregion AddOrderForTableByWorker

        #region FulfillOrder
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FulfillOrder_failes_when_worker_is_waiter()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Fulfilled = false,
                Item = null,
                Quantity = 1,
                Bill = null
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            repository.FulfillOrder(orders[0], new Employee() { Role = EmployeeRole.WAITER });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FulfillOrder_failes_if_order_already_fulfilled()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Fulfilled = true,
                Item = null,
                Quantity = 1,
                Bill = null
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            repository.FulfillOrder(orders[0], new Employee() { Role = EmployeeRole.WAITER });
        }

        [TestMethod]
        public void FulfillOrder_succeeds_for_bartender_and_cook_and_calls_save_changes()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Fulfilled = false,
                Item = null,
                Quantity = 1,
                Bill = null
            });

            var numSaveCalls = 0;

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).WithSaveChangesCallback(() => ++numSaveCalls).Create();

            var repository = new RestourantRepository(mockContext);

            repository.FulfillOrder(orders[0], new Employee() { Role = EmployeeRole.BARTENDER });

            Assert.IsTrue(orders[0].Fulfilled);
            Assert.AreEqual(1, numSaveCalls);

            numSaveCalls = 0;
            orders[0].Fulfilled = false;

            repository.FulfillOrder(orders[0], new Employee() { Role = EmployeeRole.COOK });

            Assert.IsTrue(orders[0].Fulfilled);
            Assert.AreEqual(1, numSaveCalls);
        }
        #endregion FulfillOrder

        #region CheckoutTable
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckoutTable_failes_when_worker_is_cook()
        {
            var bills = new List<Bill>();
            bills.Add(new Bill()
            {
                ID = 1,
                Table = 1,
                CheckedOut = false,
                Orders = new List<OrderItem>()
            });

            var mockContext = new MockDBContextFactory().WithBuiltinBills(bills).Create();

            var repository = new RestourantRepository(mockContext);

            repository.CheckoutTable(1, new Employee() { Role = EmployeeRole.COOK });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CheckoutTable_failes_when_no_bill()
        {
            var mockContext = new MockDBContextFactory().Create();
            var repository = new RestourantRepository(mockContext);

            repository.CheckoutTable(1, new Employee() { Role = EmployeeRole.WAITER });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CheckoutTable_failes_when_no_orders_for_table()
        {
            var bills = new List<Bill>();
            bills.Add(new Bill()
            {
                ID = 1,
                Table = 1,
                CheckedOut = false,
                Orders = new List<OrderItem>()
            });
            var mockContext = new MockDBContextFactory().WithBuiltinBills(bills).Create();
            var repository = new RestourantRepository(mockContext);

            repository.CheckoutTable(1, new Employee() { Role = EmployeeRole.BARTENDER });
        }

        [TestMethod]
        public void CheckoutTable_succeeds_for_bartender_and_waiter()
        {
            var bills = new List<Bill>();
            bills.Add(new Bill()
            {
                ID = 1,
                Table = 1,
                CheckedOut = false,
                Orders = new List<OrderItem>()
            });
            bills[0].Orders.Add(new OrderItem());

            var mockContext = new MockDBContextFactory().WithBuiltinBills(bills).Create();

            var repository = new RestourantRepository(mockContext);

            repository.CheckoutTable(1, new Employee() { Role = EmployeeRole.BARTENDER });

            Assert.IsTrue(bills[0].CheckedOut);

            bills[0].CheckedOut = false;
            bills[0].Orders.First().Fulfilled = false;

            repository.CheckoutTable(1, new Employee() { Role = EmployeeRole.WAITER });

            Assert.IsTrue(bills[0].CheckedOut);
        }

        [TestMethod]
        public void CheckoutTable_calls_save_changes()
        {
            var bills = new List<Bill>();
            bills.Add(new Bill()
            {
                ID = 1,
                Table = 1,
                CheckedOut = false,
                Orders = new List<OrderItem>()
            });
            bills[0].Orders.Add(new OrderItem());

            var numSaveCalls = 0;

            var mockContext = new MockDBContextFactory().WithBuiltinBills(bills).WithSaveChangesCallback(() => ++numSaveCalls).Create();

            var repository = new RestourantRepository(mockContext);

            repository.CheckoutTable(1, new Employee() { Role = EmployeeRole.BARTENDER });

            Assert.AreEqual(1, numSaveCalls);
        }
        #endregion CheckoutTable
    }
}
