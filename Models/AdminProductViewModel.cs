using NorthwindMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindMarket.Models
{
    public class AdminProductViewModel
    {
        public IEnumerable<SelectListItem> SelectedListCategory { get; set; }

        public IEnumerable<Products> Product { get; set; }

    }
}