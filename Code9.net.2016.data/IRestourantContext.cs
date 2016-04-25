using Code9.net._2016.data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.net._2016.data
{
    public interface IRestourantContext : IDisposable
    {
        DbSet<MenuItem> MenuItems { get; set; }
        DbSet<OrderItem> Orders { get; set; }

        // eager loading helpers
        IQueryable<OrderItem> OrdersWithMenu { get; }

        int SaveChanges();
    }
}
