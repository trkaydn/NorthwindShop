using NorthwindMarket.Models.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NorthwindMarket.Models
{
    public class AdminOrderViewModel
    {
        public PagedList<Orders> Order { get; set; }

        public IEnumerable<OrderDetails> OrderDetail { get; set; }

        public IEnumerable<Products> Product { get; set; }

    }
}