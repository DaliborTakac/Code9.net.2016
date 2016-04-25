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
        private ICollection<MenuItem> mockMenuItems { get; set; }
        private ICollection<OrderItem> mockOrderItems { get; set; }

        private Action saveChangesInvocationCallback { get; set; }

        public MockDBContextFactory()
        {
            mockMenuItems = new List<MenuItem>();
            mockOrderItems = new List<OrderItem>();
            saveChangesInvocationCallback = () => { }; //NOOP callback, needed here because the callback will be called regardless and if callback is null, calling it will thow exception
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

        public MockDBContextFactory WithSaveChangesCallback(Action callback)
        {
            saveChangesInvocationCallback = callback;
            return this;
        }

        public IRestourantContext Create()
        {
            var moq = new Mock<IRestourantContext>();

            var moqMenuItems = new Mock<DbSet<MenuItem>>();
            var menuItems = mockMenuItems.AsQueryable();
            moqMenuItems.As<IQueryable<MenuItem>>().Setup(m => m.Provider).Returns(menuItems.Provider);
            moqMenuItems.As<IQueryable<MenuItem>>().Setup(m => m.Expression).Returns(menuItems.Expression);
            moqMenuItems.As<IQueryable<MenuItem>>().Setup(m => m.ElementType).Returns(menuItems.ElementType);
            moqMenuItems.As<IQueryable<MenuItem>>().Setup(m => m.GetEnumerator()).Returns(() => {
                var it = menuItems.GetEnumerator();
                it.Reset();
                return it;
            });
            moqMenuItems.Setup(m => m.Add(It.IsAny<MenuItem>())).Callback<MenuItem>(arg => mockMenuItems.Add(arg));
            moqMenuItems.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(arg => mockMenuItems.SingleOrDefault(m => m.ID == (int)(arg[0])));
            moq.Setup(m => m.MenuItems).Returns(moqMenuItems.Object);

            var moqOrders = new Mock<DbSet<OrderItem>>();
            var orders = mockOrderItems.AsQueryable();
            moqOrders.As<IQueryable<OrderItem>>().Setup(m => m.Provider).Returns(orders.Provider);
            moqOrders.As<IQueryable<OrderItem>>().Setup(m => m.Expression).Returns(orders.Expression);
            moqOrders.As<IQueryable<OrderItem>>().Setup(m => m.ElementType).Returns(orders.ElementType);
            moqOrders.As<IQueryable<OrderItem>>().Setup(m => m.GetEnumerator()).Returns(() =>
            {
                var it = orders.GetEnumerator();
                it.Reset();
                return it;
            });
            moqOrders.Setup(m => m.Add(It.IsAny<OrderItem>())).Callback<OrderItem>(arg => mockOrderItems.Add(arg));
            moqOrders.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(arg => mockOrderItems.SingleOrDefault(o => o.ID == (int)(arg[0])));
            moq.Setup(m => m.Orders).Returns(moqOrders.Object);
            moq.Setup(m => m.OrdersWithMenu).Returns(moqOrders.Object);

            moq.Setup(m => m.SaveChanges()).Returns(1).Callback(saveChangesInvocationCallback);

            return moq.Object;
        }
    }
}
