using NorthwindMarket.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NorthwindMarket.Controllers
{
    public class LoginController : Controller
    {
        Context _context = new Context();

        public ActionResult Index()
        {
            if (Session["user"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Index(string customerID, string password)
        {
            Customers customer = _context.Customers.Where(x => x.CustomerID == customerID && x.Password == password).FirstOrDefault();
            if (customer != null && customer.Activated == true)
            {
                Session["user"] = customerID;
                FormsAuthentication.SetAuthCookie(customerID, false);
                return RedirectToAction("Index", "Home");

            }
            else if (customer != null && customer.Activated == false)
            {
                ViewBag.LoginMessage = "Hesabınız yönetici tarafından engellenmiştir.";
                return View();

            }

            ViewBag.LoginMessage = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        public ActionResult NewCustomer()
        {
            if (Session["user"] != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult NewCustomer(Customers customer)
        {
            foreach (var cst in _context.Customers)
            {
                if (cst.CustomerID == customer.CustomerID)
                {
                    ViewBag.CreateAccount = "Bu Müşteri ID'si zaten kayıtlı.";
                    return View();
                }
            }

            customer.Activated = true;
            _context.Customers.Add(customer);
            _context.SaveChanges();
            TempData["CreateAccount"] = "Hesap başarıyla oluşturuldu. Lütfen giriş yapın.";
            return RedirectToAction("Index");
        }

        public ActionResult Admin()
        {

            if (Session["admin"] != null)
                return RedirectToAction("Index", "Admin");

            return View();
        }

        [HttpPost]
        public ActionResult Admin(string username, string password)
        {
            Employees employee = _context.Employees.Where(x => x.UserName == username && x.Password == password).FirstOrDefault();
            if (employee != null)
            {
                Session["admin"] = username;
                FormsAuthentication.SetAuthCookie(username, false);
                return RedirectToAction("Index", "Admin");

            }

            ViewBag.AdminLoginMessage = "Kullanıcı adı veya şifre hatalı.";
            return View();
        }

        public ActionResult NewEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewEmployee(Employees employee)
        {
            foreach (var emp in _context.Employees)
            {
                if (emp.UserName == employee.UserName)
                {
                    ViewBag.CreateAccount = "Bu kullanıcı adı zaten kayıtlı.";
                    return View();
                }
            }

            _context.Employees.Add(employee);
            _context.SaveChanges();
            TempData["CreateAccount"] = "Hesap başarıyla oluşturuldu. Lütfen giriş yapın.";
            return RedirectToAction("Admin");
        }

    }
}