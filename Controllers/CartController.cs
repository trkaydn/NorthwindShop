using NorthwindMarket.Models;
using NorthwindMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindMarket.Controllers
{
    public class CartController : Controller
    {
        Context _context = new Context();
        public static Dictionary<Products, int> ShoppingCart;

        public ActionResult Index()
        {
            return View(Session["cart"]);
        }

        [HttpPost]
        public JsonResult AddToCart(int unit, int productID, bool changed = false)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductID == productID);

            if (product.UnitsInStock < unit ||product.Activated==false)
                return Json(0, JsonRequestBehavior.AllowGet);


            if ((Dictionary<Products, int>)Session["cart"] == null)
            {
                ShoppingCart = new Dictionary<Products, int>();
                Session["cart"] = ShoppingCart;
            }
            else
                ShoppingCart = (Dictionary<Products, int>)Session["cart"];

            //Sepette aynısı varsa Tekrar eklemesin value değerini artırsın
            foreach (var key in ShoppingCart.Keys)
            {
                if (key.ProductName == product.ProductName)
                {
                    if (changed)
                        ShoppingCart[key] = unit;
                    else
                    {
                        //stok sayısının aşılıp aşılmadığı
                        var count = ShoppingCart[key];
                        var total = count += unit;

                        if (total > product.UnitsInStock)
                            return Json(0, JsonRequestBehavior.AllowGet);

                        ShoppingCart[key] += unit;
                    }
                    Session["cart"] = ShoppingCart;

                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }

            ShoppingCart.Add(product, unit);
            Session["cart"] = ShoppingCart;
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Remove(int id)
        {
            try
            {
                ShoppingCart = (Dictionary<Products, int>)Session["cart"];

                var removingKey = ShoppingCart.Keys.Where(x => x.ProductID == id).FirstOrDefault();

                ShoppingCart.Remove(removingKey);
                Session["cart"] = ShoppingCart;
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult UpdateCart()
        {
            var cart = (Dictionary<Products, int>)Session["cart"];
            var count = cart.Keys.Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult _PartialCart()
        {
            return PartialView();
        }

        public JsonResult GetNewTotalPrice()
        {
            double totalPrice = 0;
            ShoppingCart = (Dictionary<Products, int>)Session["cart"];
            foreach (var item in ShoppingCart)
            {
                double price = Convert.ToDouble(item.Key.UnitPrice * item.Value);
                totalPrice += price;
            }
            return Json(totalPrice, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTotalWithShipping(int shipID)
        {
            var shipPrice = _context.Shippers.Where(x => x.ShipperID == shipID).FirstOrDefault().ShipPrice;
            return Json(shipPrice, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "user")]
        public ActionResult CompleteOrder()
        {

            string customerID = Session["user"].ToString();
            ShoppingCart = (Dictionary<Products, int>)Session["cart"];
            double totalPrice = 0;
            CompleteOrderViewModel model = new CompleteOrderViewModel();

            model.User = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
            model.SelectSeller = new SelectList((from s in _context.Employees select new { EmployeeID = s.EmployeeID, FullName = s.FirstName + " " + s.LastName }), "EmployeeID", "FullName");

            foreach (var item in ShoppingCart)
            {
                totalPrice += Convert.ToDouble(item.Key.UnitPrice * item.Value);
            }

            if (totalPrice >= 100)
                model.SelectShipper = new SelectList((from s in _context.Shippers select new { ShipperID = s.ShipperID, FullText = s.CompanyName + " - Ücretsiz" }), "ShipperID", "FullText");
            else
                model.SelectShipper = new SelectList((from s in _context.Shippers select new { ShipperID = s.ShipperID, FullText = s.CompanyName + " - $" + s.ShipPrice }), "ShipperID", "FullText");


            model.ShoppingCart = ShoppingCart;
            return View(model);

        }

        [HttpPost]
        public ActionResult CompleteOrder(CompleteOrderViewModel model)
        {
            string customerID = Session["user"].ToString();
            Customers customer = _context.Customers.Where(x => x.CustomerID == customerID).FirstOrDefault();
            Shippers shipper = _context.Shippers.Where(x => x.ShipperID == model.ShipperID).FirstOrDefault();

            if (string.IsNullOrEmpty(customer.Address) || string.IsNullOrEmpty(customer.City) || string.IsNullOrEmpty(customer.Country) || string.IsNullOrEmpty(customer.Phone))
            {
                TempData["AddressError"] = "Sipariş verebilmek için adres bilgilerinizi doldurunuz.";
                return RedirectToAction("AddressInfo", "User");
            }

            ShoppingCart = (Dictionary<Products, int>)Session["cart"];
            double totalprice = 0;
            decimal freight = 0;
            foreach (var item in ShoppingCart)
            {
                var price = Convert.ToDouble(item.Key.UnitPrice * item.Value);
                totalprice += price;
            }
            if (totalprice < 100)
                freight = shipper.ShipPrice;


            var order = _context.Orders.Add(new Orders
            {

                CustomerID = customerID,
                OrderDate = DateTime.Now,
                ShipVia = model.ShipperID,
                Freight = freight,
                ShipName = customer.ContactName,
                ShipAddress = customer.Address,
                ShipCity = customer.City,
                ShipRegion = customer.Region,
                ShipPostalCode = customer.PostalCode,
                ShipCountry = customer.Country,
                EmployeeID = model.EmployeeID,
                OrderStatus = true
                
            });

            foreach(var item in ShoppingCart)
            {
                _context.OrderDetails.Add(new OrderDetails
                {
                    OrderID = order.OrderID,
                    ProductID = item.Key.ProductID,
                    UnitPrice = (decimal)item.Key.UnitPrice,
                    Quantity = (short)item.Value,
                    Discount = 0
                });

                var prd = _context.Products.Where(x => x.ProductID == item.Key.ProductID).FirstOrDefault();
                prd.UnitsInStock = (short)(prd.UnitsInStock - item.Value);
                prd.UnitsOnOrder = (short)(prd.UnitsOnOrder + item.Value);
            }

            Session["cart"] = null;
            _context.SaveChanges();
            return RedirectToAction("Completed");
        }


        [Authorize(Roles = "user")]
        public ActionResult Completed()
        {
            return View();
        }

    }
}