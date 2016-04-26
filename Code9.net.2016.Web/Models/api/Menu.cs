using Code9.net._2016.data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Code9.net._2016.Web.Models.api
{
    public class Menu
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, 100000)]
        public double Price { get; set; }

        [Required]
        [EnumDataType(typeof(MenuItemKind))]
        public MenuItemKind Kind { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}