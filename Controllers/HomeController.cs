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
    public class HomeController : Controller
    {
        Context _context = new Context();
        HomeViewModel _model = new HomeViewModel();

        public HomeController()
        {
            _model.Categories = _context.Categories.Where(x => x.Activated == true);
            _model.Products = _context.Products.Where(x => x.Activated == true);
            _model.Shipper = _context.Shippers.Where(x => x.Activated == true).OrderByDescending(X => X.ShipPrice);

        }

        public ActionResult Index()
        {
            return View(_model);
        }

        public ActionResult About()
        {
            return View(_model);
        }


        public ActionResult Category(int id, int page = 1)
        {
            if (id != 0)
            {
                _model.Category = _context.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
                if (_model.Category != null)
                    _model.PagedProducts = _context.Products.Where(x => x.CategoryID == id && x.Activated == true).ToList().ToPagedList(page, 9);
                else
                    return RedirectToAction("NotFound", "Error");
            }
            else
                _model.PagedProducts = _context.Products.Where(x => x.Activated == true).ToList().ToPagedList(page, 9);

            return View(_model);
        }

        public ActionResult Search(string value)
        {
            ViewBag.Value = value;
            _model.Products = _context.Products.Where(x => x.ProductName.Contains(value) && x.Activated == true);
            return View(_model);
        }

        public ActionResult Product(int id)
        {
            var product = _context.Products.Where(x => x.ProductID == id).FirstOrDefault();
            if (!product.Activated)
                return RedirectToAction("NotAvailable", new { id = id });

            _model.Product = product;
            return View(_model);
        }

        public ActionResult NotAvailable(int id)
        {
            _model.Product = _context.Products.Where(x => x.ProductID == id).FirstOrDefault();
            return View(_model);
        }

    }
}