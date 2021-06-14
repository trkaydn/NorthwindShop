using NorthwindMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NorthwindMarket.Models
{
    public class AdminCustomerViewModel
    {
        public IEnumerable<Customers> Customer { get; set; }
    }
}