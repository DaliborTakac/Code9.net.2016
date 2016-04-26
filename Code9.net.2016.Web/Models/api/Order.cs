using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Code9.net._2016.Web.Models.api
{
    public class Order
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int MenuId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public bool Payed { get; set; }

        [Required]
        public bool Delivered { get; set; }

        [Required]
        public int Table { get; set; }
    }
}