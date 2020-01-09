using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Trashcollector.Models;

namespace Trashcollector.Controllers
{
    public class EmployeesController : Controller
    {
        
       
        ApplicationDbContext db;
        public EmployeesController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TodaysPickUp()
        {
            string datenow = System.DateTime.Now.ToString("MM/dd/yyyy");
            string day = System.DateTime.Today.DayOfWeek.ToString();
            var id = User.Identity.GetUserId();
            var usernow = db.Employees.Include(e => e.ApplicationUser).FirstOrDefault(e => e.ApplicationId == id);
            int zipcode = usernow.ZipCode;
            var customers = db.Customers.Include(e => e.ApplicationUser).Where(e => e.PickUpDay == day || e.ExtraPickUpDate == datenow && e.ZipCode == zipcode).ToList();
            return View(customers);
        }
        // GET: Employees/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            Employee employee = new Employee();
            return View(employee);
        }

        // POST: Employees/Create
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                // TODO: Add insert logic here
                employee.ApplicationId = User.Identity.GetUserId();
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("TodaysPickUp");
                
            }
            catch
            {
                return View();
            }
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Employees/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Employees/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
