using Code9.net._2016.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Code9.net._2016.Web.Models
{
    public class CheckoutModel
    {
        public int Table { get; set; }
        public IEnumerable<OrderItem> Orders { get; set; }
        public double Total { get; set; }
    }
}