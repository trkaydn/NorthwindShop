using NorthwindMarket.Models;
using NorthwindMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NorthwindMarket.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller
    {
        Context _context = new Context();
        UserViewModel _model = new UserViewModel();


        public ActionResult Index()
        {
            string customerID = Session["user"].ToString();
            _model.User = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
            _model.Order = _context.Orders.Where(x => x.CustomerID == customerID);
            return View(_model);
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            //Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserInfo()
        {
            string customerID = Session["user"].ToString();
            _model.User = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
            return View(_model);
        }

        [HttpPost]
        public ActionResult UserInfo(UserViewModel model)
        {
            var user = _context.Customers.Where(x => x.CustomerID == model.User.CustomerID).FirstOrDefault();
            user.CompanyName = model.User.CompanyName;
            user.ContactName = model.User.ContactName;
            user.ContactTitle = model.User.ContactTitle;
            _context.SaveChanges();
            _model.User = user;
            ViewBag.UserMessage = "Kullanıcı bilgileriniz başarıyla güncellendi.";
            return View(_model);
        }

        public ActionResult AddressInfo()
        {
            string customerID = Session["user"].ToString();
            _model.User = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
            return View(_model);
        }

        [HttpPost]
        public ActionResult AddressInfo(UserViewModel model)
        {
            string customerID = Session["user"].ToString();

            //bu alanlar yeni müşteri kayıt olurken zorunlu olmadığı için validation kullanmadım.
            if (string.IsNullOrEmpty(model.User.Address) || string.IsNullOrEmpty(model.User.City) || string.IsNullOrEmpty(model.User.Country) || string.IsNullOrEmpty(model.User.Phone))
            {
                _model.User = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
                ViewBag.AddressError = "Adres,Şehir,Ülke ve Telefon zorunludur.";
                return View(_model);
            }

            var user = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
            user.Address = model.User.Address;
            user.City = model.User.City;
            user.Region = model.User.Region;
            user.PostalCode = model.User.PostalCode;
            user.Country = model.User.Country;
            user.Phone = model.User.Phone;
            user.Fax = model.User.Fax;
            _context.SaveChanges();
            _model.User = user;
            ViewBag.AddressMessage = "Adres bilgileriniz başarıyla güncellendi.";
            return View(_model);
        }

        [HttpPost]
        public JsonResult ChangePassword(string oldpassword, string newpassword1, string newpassword2)
        {

            string customerID = Session["user"].ToString();
            var customer = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();

            if (customer.Password != oldpassword)
                return Json(0, JsonRequestBehavior.AllowGet);

            else if (newpassword1 != newpassword2 || string.IsNullOrEmpty(newpassword1))
                return Json(2, JsonRequestBehavior.AllowGet);


            customer.Password = newpassword1;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Orders()
        {
            string customerID = Session["user"].ToString();
            _model.User = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
            _model.Order = _context.Orders.Where(x => x.CustomerID == customerID).OrderByDescending(x => x.OrderDate);
            _model.OrderDetail = _context.OrderDetails;
            _model.Product = _context.Products;
            return View(_model);

        }

        [HttpPost]
        public JsonResult DeleteOrder(int orderID)
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
                return Json(0,JsonRequestBehavior.AllowGet);

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


        public ActionResult OrderDetail(int id)
        {
            string customerID = Session["user"].ToString();
            _model.User = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
            _model.OrderForDetailPage = _context.Orders.Where(x => x.OrderID == id).FirstOrDefault();
            _model.OrderDetail = _context.OrderDetails.Where(x => x.OrderID == id);
            return View(_model);
        }
    }
}