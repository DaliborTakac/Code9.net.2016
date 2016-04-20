using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.net._2016.data.Entities
{
    public class Bill
    {
        public int ID { get; set; }
        public int Table { get; set; }
        public bool CheckedOut { get; set; }
        public ICollection<OrderItem> Orders { get; set; }
        public Employee PersonServing { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
