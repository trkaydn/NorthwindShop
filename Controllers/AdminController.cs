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
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        Context _context = new Context();
        AdminViewModel _model = new AdminViewModel();

        public ActionResult Index()
        {
            string userName = Session["admin"].ToString();
            _model.Admin = _context.Employees.Where(x => x.UserName == userName).FirstOrDefault();
            var orders = _context.Orders.Where(x => x.EmployeeID == _model.Admin.EmployeeID);
            ViewBag.Waiting = orders.Where(x => x.ShippedDate == null && x.OrderStatus).Count();
            ViewBag.Completed = orders.Where(x => x.ShippedDate != null).Count();
            ViewBag.Cancelled = orders.Where(x => x.OrderStatus == false).Count();
            return View(_model);
        }

        public ActionResult Logout()
        {
            Session.Remove("admin");
            //Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public JsonResult ChangePassword(string oldpassword, string newpassword1, string newpassword2)
        {

            string userName = Session["admin"].ToString();
            var employee = _context.Employees.Where(x => x.UserName == userName).FirstOrDefault();

            if (employee.Password != oldpassword)
                return Json(0, JsonRequestBehavior.AllowGet);

            else if (newpassword1 != newpassword2 || string.IsNullOrEmpty(newpassword1))
                return Json(2, JsonRequestBehavior.AllowGet);


            employee.Password = newpassword1;
            _context.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddressInfo()
        {
            string userName = Session["admin"].ToString();
            _model.Admin = _context.Employees.Where(x => x.UserName == userName).FirstOrDefault();
            return View(_model);
        }

        [HttpPost]
        public ActionResult AddressInfo(AdminViewModel model)
        {
            string userName = Session["admin"].ToString();

            if (string.IsNullOrEmpty(model.Admin.Address) || string.IsNullOrEmpty(model.Admin.City) || string.IsNullOrEmpty(model.Admin.Country) || string.IsNullOrEmpty(model.Admin.HomePhone))
            {
                _model.Admin = _context.Employees.Where(x => x.UserName == userName).FirstOrDefault();
                ViewBag.AddressError = "Adres,Şehir,Ülke ve Telefon zorunludur.";
                return View(_model);
            }

            var admin = _context.Employees.Where(x => x.UserName == userName).FirstOrDefault();
            admin.Address = model.Admin.Address;
            admin.City = model.Admin.City;
            admin.Region = model.Admin.Region;
            admin.PostalCode = model.Admin.PostalCode;
            admin.Country = model.Admin.Country;
            admin.HomePhone = model.Admin.HomePhone;
            _context.SaveChanges();
            _model.Admin = admin;
            ViewBag.AddressMessage = "İletişim bilgileriniz başarıyla güncellendi.";
            return View(_model);
        }

        public ActionResult AdminInfo()
        {
            string userName = Session["admin"].ToString();
            _model.Admin = _context.Employees.Where(x => x.UserName == userName).FirstOrDefault();
            return View(_model);
        }

        [HttpPost]
        public ActionResult AdminInfo(AdminViewModel model)
        {
            var admin = _context.Employees.Where(x => x.UserName == model.Admin.UserName).FirstOrDefault();
            if (admin != null)
            {
                admin.FirstName = model.Admin.FirstName;
                admin.LastName = model.Admin.LastName;
                admin.Title = model.Admin.Title;
                admin.Notes = model.Admin.Notes;
                admin.BirthDate = model.Admin.BirthDate;
                admin.HireDate = model.Admin.HireDate;
                _context.SaveChanges();
                _model.Admin = admin;
                ViewBag.AdminMessage = "Kişisel bilgileriniz başarıyla güncellendi.";
                return View(_model);
            }
            else
                return RedirectToAction("AdminInfo");

        }

    }
}