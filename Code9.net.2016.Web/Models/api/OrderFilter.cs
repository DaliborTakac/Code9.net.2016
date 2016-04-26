using Code9.net._2016.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Code9.net._2016.Web.Models.api
{
    public class OrderFilter
    {
        public int? Table { get; set; }
        public MenuItemKind? Kind { get; set; }
    }
}