using NorthwindMarket.Models;
using NorthwindMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindMarket.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminProductController : Controller
    {
        Context _context = new Context();
        AdminProductViewModel _model = new AdminProductViewModel();

        public AdminProductController()
        {
            _model.Product = _context.Products;
            _model.SelectedListCategory = new SelectList(_context.Categories, "CategoryID", "CategoryName");
        }

        public ActionResult Index()
        {
            return View(_model);
        }

        public PartialViewResult Products(int? categoryID, int? status)
        {
            if (categoryID != null)
                _model.Product = _model.Product.Where(x => x.CategoryID == categoryID);
            if (status != null && status == 0)
                _model.Product = _model.Product.Where(x => x.Activated == false);
            else if (status != null && status == 1)
                _model.Product = _model.Product.Where(x => x.Activated);

            return PartialView(_model);
        }

        public JsonResult Enable(int productID)
        {
            var product = _context.Products.Where(x => x.ProductID == productID).FirstOrDefault();
            product.Activated = true;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Disable(int productID)
        {
            var product = _context.Products.Where(x => x.ProductID == productID).FirstOrDefault();
            product.Activated = false;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);

        }

        public JsonResult EditPrice(int productID)
        {
            var product = _context.Products.Where(x => x.ProductID == productID).FirstOrDefault();
            List<string> properties = new List<string>();
            properties.Add(product.ProductID.ToString());
            properties.Add(product.ProductName);
            properties.Add(product.UnitPrice.ToString());
            properties.Add(product.UnitsInStock.ToString());
            return Json(properties, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditPrice(int productID, string UnitPrice, int UnitsInStock)
        {
            if (UnitPrice == "" || UnitsInStock < 0)
                return Json(0, JsonRequestBehavior.AllowGet);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            decimal UnitPriceNumber = Convert.ToDecimal(UnitPrice, provider);

            if (UnitPriceNumber <= 0)
                return Json(0, JsonRequestBehavior.AllowGet);


            var updatingProduct = _context.Products.Where(x => x.ProductID == productID).FirstOrDefault();
            updatingProduct.UnitsInStock = (short)UnitsInStock;
            updatingProduct.UnitPrice = UnitPriceNumber;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditDetail(int id)
        {
            var product = _context.Products.Where(x => x.ProductID == id).FirstOrDefault();
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierID", "CompanyName");
            return View(product);
        }

        [HttpPost]
        public ActionResult EditDetail(Products product)
        {
            var updatingProduct = _context.Products.Where(x => x.ProductID == product.ProductID).FirstOrDefault();
            updatingProduct.ProductName = product.ProductName;
            updatingProduct.QuantityPerUnit = product.QuantityPerUnit;
            updatingProduct.CategoryID = product.CategoryID;
            updatingProduct.SupplierID = product.SupplierID;
            _context.SaveChanges();
            TempData["Message"] = "'" + product.ProductName + "'" + " ürün detayları başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        public ActionResult Add()
        {
            Products product = new Products();
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "SupplierID", "CompanyName");
            return View(product);
        }

        [HttpPost]
        public ActionResult Add(Products product, string unitPrice)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            decimal UnitPriceNumber = Convert.ToDecimal(unitPrice, provider);

            product.UnitPrice = UnitPriceNumber;
            product.UnitsOnOrder = 0;
            product.ReorderLevel = 0;
            _context.Products.Add(product);
            _context.SaveChanges();
            TempData["Message"] = "'" + product.ProductName + "'" + " ürünü başarıyla eklendi.";
            return RedirectToAction("Index");

        }


    }
}