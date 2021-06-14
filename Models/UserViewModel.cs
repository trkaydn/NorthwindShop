using NorthwindMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NorthwindMarket.Models
{
    public class UserViewModel
    {
        public Customers User { get; set; }

        public IEnumerable<Orders> Order { get; set; }

        public IEnumerable<OrderDetails> OrderDetail { get; set; }

        public Orders OrderForDetailPage { get; set; }
        public IEnumerable<Products> Product { get; set; }

    }
}