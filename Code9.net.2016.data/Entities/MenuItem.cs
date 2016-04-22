using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.net._2016.data.Entities
{
    public class MenuItem
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public MenuItemKind Kind { get; set; }
        public double Price { get; set; }
        public bool Active { get; set; }
    }
}
