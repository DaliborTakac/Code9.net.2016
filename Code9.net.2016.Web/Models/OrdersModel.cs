using Code9.net._2016.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Code9.net._2016.Web.Models
{
    public class Order
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool Delivered { get; set; }
    }

    public class OrderGroup
    {
        public string GroupPrefix { get; set; }
        public IList<Order> Orders { get; set; }
    }

    public class OrdersModel
    {
        public Table Table { get; set; }
        public OrderGroup Drinks { get; set; }
        public OrderGroup Meals { get; set; }
    }
}