namespace Code9.net._2016.data
{
    using Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class RestourantContext : DbContext, IRestourantContext
    {
        // Your context has been configured to use a 'Restourant' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Code9.net._2016.DAL.Restourant' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Restourant' 
        // connection string in the application configuration file.
        public RestourantContext()
            : base("name=Restourant")
        {
        }

        public virtual DbSet<Employee> Workers { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<OrderItem> Orders { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }

        public IQueryable<Bill> BillsWithOrdersWithMenu
        {
            get
            {
                return Bills.Include(b => b.Orders.Select(o => o.Item));
            }
        }

        public IQueryable<OrderItem> OrdersWithMenu
        {
            get
            {
                return Orders.Include(o => o.Item);
            }
        }
    }
}