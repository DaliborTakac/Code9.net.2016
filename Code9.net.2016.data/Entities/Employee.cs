using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code9.net._2016.data.Entities
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public EmployeeRole Role { get; set; }
    }
}
