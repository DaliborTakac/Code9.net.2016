using Code9.net._2016.data;
using Code9.net._2016.data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.net._2016.Repositories.TEST
{
    class MockDBContextFactory
    {
        private ICollection<Employee> mockWorkers { get; set; }
        private ICollection<MenuItem> mockMenuItems { get; set; }
        private ICollection<OrderItem> mockOrderItems { get; set; }
        private ICollection<Bill> mockBills { get; set; }

        private Action saveChangesInvocationCallback { get; set; }

        public MockDBContextFactory()
        {
            mockWorkers = new List<Employee>();
            mockMenuItems = new List<MenuItem>();
            mockOrderItems = new List<OrderItem>();
            mockBills = new List<Bill>();
            saveChangesInvocationCallback = () => { }; //NOOP callback, neede here because the callback will be called regardless and if callback is null, calling it will thow exception
        }

        public MockDBContextFactory WithBuiltinWorkers(ICollection<Employee> workers)
        {
            mockWorkers = workers;
            return this;
        }

        public MockDBContextFactory WithBuiltinMenuItems(ICollection<MenuItem> menu)
        {
            mockMenuItems = menu;
            return this;
        }

        public MockDBContextFactory WithBuiltinOrderItems(ICollection<OrderItem> orders)
        {
            mockOrderItems = orders;
            return this;
        }

        public MockDBContextFactory WithBuiltinBills(ICollection<Bill> bills)
        {
            mockBills = bills;
            return this;
        }

        public MockDBContextFactory WithSaveChangesCallback(Action callback)
        {
            saveChangesInvocationCallback = callback;
            return this;
        }

        public IRestourantContext Create()
        {
            var moq = new Mock<IRestourantContext>();

            var moqWorkers = new Mock<DbSet<Employee>>();
            var workers = mockWorkers.AsQueryable();
            moqWorkers.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(workers.Provider);
            moqWorkers.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(workers.Expression);
            moqWorkers.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(workers.ElementType);
            moqWorkers.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(workers.GetEnumerator());
            moqWorkers.Setup(m => m.Add(It.IsAny<Employee>())).Callback<Employee>(arg => mockWorkers.Add(arg));
            moq.Setup(m => m.Workers).Returns(moqWorkers.Object);

            var moqMenuItems = new Mock<DbSet<MenuItem>>();
            var menuItems = mockMenuItems.AsQueryable();
            moqMenuItems.As<IQueryable<MenuItem>>().Setup(m => m.Provider).Returns(menuItems.Provider);
            moqMenuItems.As<IQueryable<MenuItem>>().Setup(m => m.Expression).Returns(menuItems.Expression);
            moqMenuItems.As<IQueryable<MenuItem>>().Setup(m => m.ElementType).Returns(menuItems.ElementType);
            moqMenuItems.As<IQueryable<MenuItem>>().Setup(m => m.GetEnumerator()).Returns(menuItems.GetEnumerator());
            moqMenuItems.Setup(m => m.Add(It.IsAny<MenuItem>())).Callback<MenuItem>(arg => mockMenuItems.Add(arg));
            moq.Setup(m => m.MenuItems).Returns(moqMenuItems.Object);

            var moqOrders = new Mock<DbSet<OrderItem>>();
            var orders = mockOrderItems.AsQueryable();
            moqOrders.As<IQueryable<OrderItem>>().Setup(m => m.Provider).Returns(orders.Provider);
            moqOrders.As<IQueryable<OrderItem>>().Setup(m => m.Expression).Returns(orders.Expression);
            moqOrders.As<IQueryable<OrderItem>>().Setup(m => m.ElementType).Returns(orders.ElementType);
            moqOrders.As<IQueryable<OrderItem>>().Setup(m => m.GetEnumerator()).Returns(orders.GetEnumerator());
            moqOrders.Setup(m => m.Add(It.IsAny<OrderItem>())).Callback<OrderItem>(arg => mockOrderItems.Add(arg));
            moq.Setup(m => m.Orders).Returns(moqOrders.Object);

            var moqBills = new Mock<DbSet<Bill>>();
            var bills = mockBills.AsQueryable();
            moqBills.As<IQueryable<Bill>>().Setup(m => m.Provider).Returns(bills.Provider);
            moqBills.As<IQueryable<Bill>>().Setup(m => m.Expression).Returns(bills.Expression);
            moqBills.As<IQueryable<Bill>>().Setup(m => m.ElementType).Returns(bills.ElementType);
            moqBills.As<IQueryable<Bill>>().Setup(m => m.GetEnumerator()).Returns(bills.GetEnumerator());
            moqBills.Setup(m => m.Add(It.IsAny<Bill>())).Callback<Bill>(arg => mockBills.Add(arg));
            moq.Setup(m => m.Bills).Returns(moqBills.Object);

            moq.Setup(m => m.SaveChanges()).Returns(1).Callback(saveChangesInvocationCallback);

            return moq.Object;
        }
    }
}
