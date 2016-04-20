using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.net._2016.data.Entities
{
    public class OrderItem
    {
        public int ID { get; set; }
        public MenuItem Item { get; set; }
        public int Quantity { get; set; }
        public bool Fulfilled { get; set; }
        public virtual Bill Bill { get; set; }
    }
}
