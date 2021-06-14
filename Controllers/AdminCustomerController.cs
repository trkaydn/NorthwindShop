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
    public class AdminCustomerController : Controller
    {
        Context _context = new Context();
        AdminCustomerViewModel _model = new AdminCustomerViewModel();

        public AdminCustomerController()
        {
            _model.Customer = _context.Customers;
        }

        public ActionResult Index()
        {
            return View(_model);
        }

        public PartialViewResult Customers(int? status)
        {
            if (status != null && status == 0)
                _model.Customer = _model.Customer.Where(x => x.Activated == false);
            else if (status != null && status == 1)
                _model.Customer = _model.Customer.Where(x => x.Activated);
            return PartialView(_model);
        }

        public JsonResult Disable(string CustomerID)
        {
            var shipper = _context.Customers.Where(x => x.CustomerID == CustomerID).FirstOrDefault();
            shipper.Activated = false;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Enable(string CustomerID)
        {
            var customer = _context.Customers.Where(x => x.CustomerID == CustomerID).FirstOrDefault();
            customer.Activated = true;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);

        }
    }
}