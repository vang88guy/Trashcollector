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
            var employees = db.Employees;
            return View(employees);
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
            var userid = User.Identity.GetUserId();
            var employee = db.Employees.Include(c => c.ApplicationUser).SingleOrDefault(c => c.ApplicationId == userid);
            bool role = User.IsInRole("Employee");
    
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

                return RedirectToAction("TodaysPickUp");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int id)
        {
            var employee = db.Employees.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Employee employee)
        {
            try
            {
                // TODO: Add delete logic here
                employee = db.Employees.SingleOrDefault(c => c.Id == id);
                var userdelete = db.Users.SingleOrDefault(c => c.Id == employee.ApplicationId);
                employee.ApplicationId = null;
                db.Employees.Remove(employee);
                db.Users.Remove(userdelete);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
