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
    public class AdminShipperController : Controller
    {
        Context _context = new Context();
        AdminShipperViewModel _model = new AdminShipperViewModel();

        public AdminShipperController()
        {
            _model.Shipper = _context.Shippers;
        }
        public ActionResult Index()
        {
            return View(_model);
        }

        public PartialViewResult Shippers(int? status)
        {
            if (status != null && status == 0)
                _model.Shipper = _model.Shipper.Where(x => x.Activated == false);
            else if (status != null && status == 1)
                _model.Shipper = _model.Shipper.Where(x => x.Activated);
            return PartialView(_model);
        }

        public JsonResult Disable(int ShipperID)
        {
            var shipper = _context.Shippers.Where(x => x.ShipperID == ShipperID).FirstOrDefault();
            shipper.Activated = false;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Enable(int ShipperID)
        {
            var shipper = _context.Shippers.Where(x => x.ShipperID == ShipperID).FirstOrDefault();
            shipper.Activated = true;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Add(Shippers shipper, string price)
        {
            if (price=="NaN")
                return Json(0);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            decimal shipPrice = Convert.ToDecimal(price, provider);
            shipper.ShipPrice = shipPrice;
            if (ModelState.IsValid)
            {
                foreach (var item in _context.Shippers)
                {
                    if (item.CompanyName == shipper.CompanyName)
                        return Json(2);

                }
                _context.Shippers.Add(shipper);
                _context.SaveChanges();
                return Json(1);
            }
            return Json(0);
        }
    }
}