using NorthwindMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindMarket.Models
{
    public class CompleteOrderViewModel
    {
        public Customers User { get; set; }
        public IEnumerable<SelectListItem> SelectShipper { get; set; }
        public IEnumerable<SelectListItem> SelectSeller { get; set; }

        [Required(ErrorMessage ="Lüten bir kargo firması seçin.")]
        public int ShipperID { get; set; }
        [Required(ErrorMessage = "Lüten bir gönderici seçin.")]
        public int EmployeeID { get; set; }

        public Dictionary<Products, int> ShoppingCart { get; set; }

    }
}