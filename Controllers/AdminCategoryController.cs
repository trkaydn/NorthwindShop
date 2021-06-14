using NorthwindMarket.Models;
using NorthwindMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NorthwindMarket.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminCategoryController : Controller
    {
        Context _context = new Context();
        AdminCategoryViewModel _model = new AdminCategoryViewModel();
        public AdminCategoryController()
        {
            _model.Category = _context.Categories;
        }

        public ActionResult Index()
        {
            return View(_model);
        }

        public PartialViewResult Categories(int? status)
        {
            if (status != null && status == 0)
                _model.Category = _model.Category.Where(x => x.Activated == false);
            else if (status != null && status == 1)
                _model.Category = _model.Category.Where(x => x.Activated);
            return PartialView(_model);
        }


        public JsonResult Disable(int CategoryID)
        {
            var category = _context.Categories.Where(x => x.CategoryID == CategoryID).FirstOrDefault();
            category.Activated = false;
            var products = _context.Products.Where(x => x.CategoryID == CategoryID);
            foreach (var item in products)
            {
                item.Activated = false;
            }
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);

        }


        public JsonResult Enable(int CategoryID)
        {
            var category = _context.Categories.Where(x => x.CategoryID == CategoryID).FirstOrDefault();
            category.Activated = true;
            var products = _context.Products.Where(x => x.CategoryID == CategoryID);
            foreach (var item in products)
            {
                item.Activated = true;
            }
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Add(Categories category)
        {
            if (ModelState.IsValid)
            {
                foreach (var ctg in _context.Categories)
                {
                    if (ctg.CategoryName == category.CategoryName)
                        return Json(2);

                }
                _context.Categories.Add(category);
                _context.SaveChanges();
                return Json(1);
            }
            return Json(0);
        }
    }
}