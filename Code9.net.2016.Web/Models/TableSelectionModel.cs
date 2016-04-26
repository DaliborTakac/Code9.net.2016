using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Code9.net._2016.Web.Models
{
    public class Table
    {
        public string DisplayName { get; set; }
        public int Number { get; set; }
    }

    public class TableSelectionModel
    {
        public IEnumerable<Table> Tables { get; set; }
        public Table Selected { get; set; }
    }
}