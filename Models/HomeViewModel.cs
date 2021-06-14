using NorthwindMarket.Models.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NorthwindMarket.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<Categories> Categories { get; set; }

        public Categories Category { get; set; }

        public Products Product { get; set; }
        public IPagedList<Products> PagedProducts { get; set; }
        public IEnumerable<Shippers> Shipper { get; set; }


    }
}