
using NorthwindMarket.Models;
using NorthwindMarket.Models.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindMarket.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminOrderController : Controller
    {
        Context _context = new Context();
        AdminOrderViewModel _model = new AdminOrderViewModel();
        public ActionResult Orders(string status, int page = 1)
        {
            string userName = Session["admin"].ToString();
            var admin = _context.Employees.Where(x => x.UserName == userName).FirstOrDefault();
            var orders = _context.Orders.Where(x => x.EmployeeID == admin.EmployeeID).OrderByDescending(x => x.OrderDate);
            _model.OrderDetail = _context.OrderDetails;
            _model.Product = _context.Products;
            ViewBag.status = status; 
            switch (status)
            {
                case "waiting":
                    _model.Order = (PagedList<Orders>)orders.Where(x => x.ShippedDate == null && x.OrderStatus).ToList().ToPagedList(page, 6);
                    ViewBag.Title = "Sevk Bekleyen Siparişler (" + orders.Where(x => x.ShippedDate == null && x.OrderStatus).Count() + ")";
                    break;
                case "completed":
                    _model.Order = (PagedList<Orders>)orders.Where(x => x.ShippedDate != null).ToList().ToPagedList(page, 6);
                    ViewBag.Title = "Tamamlanan Siparişler (" + orders.Where(x => x.ShippedDate != null).Count() + ")";
                    break;
                case "cancelled":
                    _model.Order = (PagedList<Orders>)orders.Where(x => x.OrderStatus == false).ToList().ToPagedList(page, 6);
                    ViewBag.Title = "İptal Edilen Siparişler (" + orders.Where(x => x.OrderStatus == false).Count() + ")";
                    break;
            }
            return View(_model);

        }

        public ActionResult DeleteOrder(int orderID)
        {
            IEnumerable<OrderDetails> details = _context.OrderDetails.Where(x => x.OrderID == orderID);
            var order = _context.Orders.Where(x => x.OrderID == orderID).FirstOrDefault();

            foreach (var removingDetail in details)
            {
                _context.OrderDetails.Remove(removingDetail);
            }
            _context.Orders.Remove(order);

            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CancelOrder(int orderID)
        {
            var order = _context.Orders.Where(x => x.OrderID == orderID).FirstOrDefault();
            if (order.OrderStatus == false)
                return Json(0, JsonRequestBehavior.AllowGet);

            IEnumerable<OrderDetails> details = _context.OrderDetails.Where(x => x.OrderID == orderID);

            //stoğu yerine koy
            foreach (var detail in details)
            {
                var product = _context.Products.Where(x => x.ProductID == detail.ProductID).First();
                product.UnitsInStock += detail.Quantity;
                product.UnitsOnOrder -= detail.Quantity;
            }
            order.OrderStatus = false;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetOrder(int orderID)
        {
            var order = _context.Orders.Where(x => x.OrderID == orderID).FirstOrDefault();
            var customer = _context.Customers.Where(x => x.CustomerID == order.CustomerID).FirstOrDefault();
            List<string> details = new List<string>();
            details.Add(order.Shippers.CompanyName);
            details.Add(customer.ContactName);
            details.Add(customer.CompanyName);
            details.Add(customer.Address);
            details.Add(customer.City);
            details.Add(customer.Country);
            details.Add(customer.PostalCode);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AcceptOrder(int orderID)
        {
            var order = _context.Orders.Where(x => x.OrderID == orderID).FirstOrDefault();
            if (order.OrderStatus == false || order.ShippedDate != null)
                return Json(0, JsonRequestBehavior.AllowGet);

            order.ShippedDate = DateTime.Now;
            _context.SaveChanges();
            return Json(1);

        }



    }
}