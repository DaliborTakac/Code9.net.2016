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

        #region GetOpenOrdersForTable
        [TestMethod]
        public void GetOpenOrdersForTable_returns_empty_list_when_all_bills_are_payed()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Item = null,
                Quantity = 1,
                Delivered = true,
            });
            orders.Add(new OrderItem()
            {
                ID = 2,
                Item = null,
                Quantity = 10,
                Delivered = true,
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            var openOrder = repository.GetOpenOrdersForTable(4);

            Assert.AreEqual(0, openOrder.Count());
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
                Delivered = false,
                Table = 4
            });
            orders.Add(new OrderItem()
            {
                ID = 2,
                Item = null,
                Quantity = 10,
                Delivered = false,
                Table = 6
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

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
                Delivered = true
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
                Delivered = true
            });
            orders.Add(new OrderItem()
            {
                ID = 8,
                Item = menu[0],
                Quantity = 2,
                Delivered = false
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
                Price = 10.00,
                Active = true,
            });
            menu.Add(new MenuItem()
            {
                DisplayName = "test meal",
                Kind = MenuItemKind.MEAL,
                ID = 5,
                Price = 25.00,
                Active = true
            });

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).Create();

            var repository = new RestourantRepository(mockContext);

            var drink = repository.GetMenuForItemKind(MenuItemKind.DRINK);
            var meal = repository.GetMenuForItemKind(MenuItemKind.MEAL);

            Assert.AreEqual(2, drink.First().ID);
            Assert.AreEqual(5, meal.First().ID);
        }
        #endregion GetMenuForItemKind

        #region GetMenuItemByID
        [TestMethod]
        public void GetMenuItemByID_returns_null_when_no_matching_item_is_found()
        {
            var menuitems = new List<MenuItem>();
            menuitems.Add(new MenuItem()
            {
                DisplayName = "test item",
                ID = 6,
                Kind = MenuItemKind.DRINK,
                Price = 10.00
            });

            var context = new MockDBContextFactory().WithBuiltinMenuItems(menuitems).Create();

            var repository = new RestourantRepository(context);

            var m = repository.GetMenuItemByID(2);

            Assert.IsNull(m);
        }

        [TestMethod]
        public void GetMenuItemByID_returnes_appropriate_item()
        {
            var menuitems = new List<MenuItem>();
            menuitems.Add(new MenuItem()
            {
                DisplayName = "test item",
                ID = 8,
                Kind = MenuItemKind.DRINK,
                Price = 10.00
            });
            menuitems.Add(new MenuItem()
            {
                DisplayName = "test item 2",
                ID = 6,
                Kind = MenuItemKind.DRINK,
                Price = 10.00
            });

            var context = new MockDBContextFactory().WithBuiltinMenuItems(menuitems).Create();

            var repository = new RestourantRepository(context);

            var m = repository.GetMenuItemByID(8);

            Assert.IsNotNull(m);
            Assert.AreEqual(8, m.ID);
        }
        #endregion GetMenuItemByID

        #region GetOrderByID
        [TestMethod]
        public void GetOrderByID_returns_null_if_no_item_with_matching_id_exists()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1
            });
            orders.Add(new OrderItem()
            {
                ID = 5
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            var order = repository.GetOrderByID(2);

            Assert.IsNull(order);
        }

        [TestMethod]
        public void GetOrderByID_returns_matching_item()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1
            });
            orders.Add(new OrderItem()
            {
                ID = 5
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            var order = repository.GetOrderByID(5);

            Assert.IsNotNull(order);
            Assert.AreEqual(5, order.ID);
        }
        #endregion GetOrderByID

        #region AddMenuItemToMenu
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddMenuItemToMenu_failes_if_worker_is_waiter()
        {
            var mockContext = new MockDBContextFactory().Create();

            var repository = new RestourantRepository(mockContext);

            repository.AddMenuItemToMenu("invalid operation generating item", 5.00, MenuItemKind.DRINK, EmployeeRole.WAITER);
        }

        [TestMethod]
        public void AddMenuItemToMenu_adds_menu_item_if_worker_is_not_waiter()
        {
            var numSaveChangesCalls = 0;
            var mockContext = new MockDBContextFactory().WithSaveChangesCallback(() => ++numSaveChangesCalls).Create();

            var repository = new RestourantRepository(mockContext);

            const string validItem1 = "valid item";

            repository.AddMenuItemToMenu(validItem1, 10.00, MenuItemKind.DRINK, EmployeeRole.BARTENDER);

            Assert.AreEqual(1, mockContext.MenuItems.Count());
            Assert.AreEqual(validItem1, mockContext.MenuItems.Last().DisplayName);
            Assert.IsTrue(0 < numSaveChangesCalls);

            numSaveChangesCalls = 0;

            const string validItem2 = "another valid item";

            repository.AddMenuItemToMenu(validItem2, 97.99, MenuItemKind.MEAL, EmployeeRole.COOK);

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

            repository.AddMenuItemToMenu(itemName, itemPrice, itemKind, EmployeeRole.BARTENDER);

            var inserted = mockContext.MenuItems.Last();

            Assert.AreEqual(itemName, inserted.DisplayName);
            Assert.AreEqual(itemPrice, inserted.Price);
            Assert.AreEqual(itemKind, inserted.Kind);
        }
        #endregion

        #region AddOrderForTableByWorker
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOrdersForTableByWorker_failes_when_worker_is_cook()
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

            var orders = new List<Models.SubmittedOrderItem>();
            orders.Add(new Models.SubmittedOrderItem() { MenuItemID = menu[0].ID, Quantity = 1 });

            repository.AddOrdersForTableByWorker(orders, 1, EmployeeRole.COOK);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddOrdersForTableByWorker_failes_when_invalid_order_id_specified()
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

            var orders = new List<Models.SubmittedOrderItem>();
            orders.Add(new Models.SubmittedOrderItem() { MenuItemID = 15, Quantity = 1 });

            repository.AddOrdersForTableByWorker(orders, 1, EmployeeRole.WAITER);
        }

        [TestMethod]
        public void AddOrdersForTableByWorker_persists_data_by_calling_save_changes_on_db_context()
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

            var orders = new List<Models.SubmittedOrderItem>();
            orders.Add(new Models.SubmittedOrderItem() { MenuItemID = menu[0].ID, Quantity = 1 });

            repository.AddOrdersForTableByWorker(orders, 1, EmployeeRole.WAITER);

            Assert.AreEqual(1, numSaveCalls);
            Assert.AreEqual(menu[0].ID, mockContext.OrdersWithMenu.First().Item.ID);
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
                Delivered = false,
                Item = null,
                Quantity = 1
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            repository.FulfillOrder(orders[0].ID, EmployeeRole.WAITER);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FulfillOrder_failes_if_order_already_fulfilled()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Delivered = true,
                Item = null,
                Quantity = 1
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            repository.FulfillOrder(orders[0].ID, EmployeeRole.COOK);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FulfillOrder_failes_when_invalid_order_id_specified()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Delivered = true,
                Item = null,
                Quantity = 1
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            repository.FulfillOrder(15, EmployeeRole.COOK);
        }

        [TestMethod]
        public void FulfillOrder_succeeds_for_bartender_and_waiter_and_calls_save_changes()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Delivered = false,
                Item = null,
                Quantity = 1
            });

            var numSaveCalls = 0;

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).WithSaveChangesCallback(() => ++numSaveCalls).Create();

            var repository = new RestourantRepository(mockContext);

            repository.FulfillOrder(orders[0].ID, EmployeeRole.BARTENDER);

            Assert.IsTrue(orders[0].Delivered);
            Assert.AreEqual(1, numSaveCalls);

            numSaveCalls = 0;
            orders[0].Delivered = false;

            repository.FulfillOrder(orders[0].ID, EmployeeRole.WAITER);

            Assert.IsTrue(orders[0].Delivered);
            Assert.AreEqual(1, numSaveCalls);
        }
        #endregion FulfillOrder

        #region DeleteOrder
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteOrder_failes_if_order_id_invalid()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Quantity = 2,
                Delivered = false
            });

            var context = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(context);

            repository.DeleteOrder(4, EmployeeRole.WAITER);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DeleteOrder_failes_id_order_fulfilled()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 1,
                Quantity = 2,
                Delivered = true
            });

            var context = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(context);

            repository.DeleteOrder(1, EmployeeRole.WAITER);
        }

        [TestMethod]
        public void DeleteOrder_sets_quantity_to_0_and_fulfills_order_and_calls_save_changes()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem()
            {
                ID = 6,
                Quantity = 2,
                Delivered = false
            });

            var numSaveCalls = 0;

            var context = new MockDBContextFactory().WithBuiltinOrderItems(orders).WithSaveChangesCallback(() => ++numSaveCalls).Create();

            var repository = new RestourantRepository(context);

            repository.DeleteOrder(6, EmployeeRole.WAITER);

            Assert.AreEqual(0, orders[0].Quantity);
            Assert.IsTrue(orders[0].Delivered);
            Assert.AreEqual(1, numSaveCalls);
        }
        #endregion DeleteOrder

        #region CheckoutTable
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckoutTable_failes_when_worker_is_cook()
        {

            var mockContext = new MockDBContextFactory().Create();

            var repository = new RestourantRepository(mockContext);

            repository.CheckoutTable(1, EmployeeRole.COOK);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CheckoutTable_failes_when_no_orders_for_table()
        {
            var mockContext = new MockDBContextFactory().Create();
            var repository = new RestourantRepository(mockContext);

            repository.CheckoutTable(1, EmployeeRole.BARTENDER);
        }

        [TestMethod]
        public void CheckoutTable_succeeds_for_bartender_and_waiter()
        {
            var orders = new List<OrderItem>()
            {
                new OrderItem()
                {
                    Table = 1, Quantity = 3
                },
                new OrderItem()
                {
                    Table = 2, Quantity = 2
                }
            };

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();

            var repository = new RestourantRepository(mockContext);

            repository.CheckoutTable(1, EmployeeRole.BARTENDER);

            Assert.IsTrue(orders[0].Payed);
            Assert.IsFalse(orders[1].Payed);

            orders[0].Payed = false;

            repository.CheckoutTable(2, EmployeeRole.WAITER);

            Assert.IsFalse(orders[0].Payed);
            Assert.IsTrue(orders[1].Payed);
        }

        [TestMethod]
        public void CheckoutTable_calls_save_changes()
        {
            var orders = new List<OrderItem>(0)
            {
                new OrderItem()
                {
                    Table = 2,
                    Quantity = 10
                }
            };

            var numSaveCalls = 0;

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).WithSaveChangesCallback(() => ++numSaveCalls).Create();

            var repository = new RestourantRepository(mockContext);

            repository.CheckoutTable(2, EmployeeRole.BARTENDER);

            Assert.AreEqual(1, numSaveCalls);
        }
        #endregion CheckoutTable

        #region UpdateMenuItem
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateMenuItem_failes_when_item_does_not_exist()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                ID = 1,
                Active = true,
                DisplayName = "test",
                Kind = MenuItemKind.DRINK,
                Price = 100.00
            });

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).Create();

            var repository = new RestourantRepository(mockContext);

            repository.UpdateMenuItem(new MenuItem() { ID = 5 });
        }

        [TestMethod]
        public void UpdatemenuItem_updates_all_fields_and_calls_save_changes()
        {
            var menu = new List<MenuItem>();
            menu.Add(new MenuItem()
            {
                ID = 1,
                Active = true,
                DisplayName = "test",
                Kind = MenuItemKind.DRINK,
                Price = 100.00
            });

            var numSaveCalls = 0;

            var mockContext = new MockDBContextFactory().WithBuiltinMenuItems(menu).WithSaveChangesCallback(() => ++numSaveCalls).Create();

            var repository = new RestourantRepository(mockContext);

            var itemToUpdate = new MenuItem()
            {
                DisplayName = "test2", Active = true, Kind = MenuItemKind.MEAL, Price = 299.99, ID = 1
            };

            repository.UpdateMenuItem(itemToUpdate);

            Assert.AreEqual(itemToUpdate.DisplayName, menu[0].DisplayName);
            Assert.AreEqual(itemToUpdate.Active, menu[0].Active);
            Assert.AreEqual(itemToUpdate.Kind, menu[0].Kind);
            Assert.AreEqual(itemToUpdate.Price, menu[0].Price);
            Assert.AreEqual(1, numSaveCalls);
        }
        #endregion UpdateMenuItem

        #region UpdateOrderItem
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateOrderItem_fails_when_item_has_ivalid_id()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem
            {
                ID = 1,
                Delivered = false,
                Item = null,
                Payed = false,
                Quantity = 1,
                Table = 12
            });

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).Create();
            var repository = new RestourantRepository(mockContext);

            repository.UpdateOrderItem(new OrderItem() { ID = 2 });
        }

        [TestMethod]
        public void UpdateOrderItem_updates_all_fields_and_calls_save_changes()
        {
            var orders = new List<OrderItem>();
            orders.Add(new OrderItem
            {
                ID = 1,
                Delivered = false,
                Item = null,
                Payed = false,
                Quantity = 1,
                Table = 12
            });

            var numSaveCalls = 0;

            var mockContext = new MockDBContextFactory().WithBuiltinOrderItems(orders).WithSaveChangesCallback(() => ++numSaveCalls).Create();

            var repository = new RestourantRepository(mockContext);

            repository.UpdateOrderItem(new OrderItem() { ID = 1, Delivered = true, Item = new MenuItem(), Payed = true, Quantity = 2, Table = 5 });

            Assert.IsTrue(orders[0].Delivered);
            Assert.IsNotNull(orders[0].Item);
            Assert.IsTrue(orders[0].Payed);
            Assert.AreEqual(2, orders[0].Quantity);
            Assert.AreEqual(5, orders[0].Table);
            Assert.AreEqual(1, numSaveCalls);
        }
        #endregion UpdateOrderItem
    }
}
