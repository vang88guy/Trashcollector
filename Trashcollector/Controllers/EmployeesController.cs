using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Trashcollector.Models;
using System.Threading.Tasks;
using static Trashcollector.Models.PickByDayViewModels;
using System.Net;
using System.Xml.Linq;

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

        public ActionResult PickByDay()
        {
            List<string> WeekDays = new List<string> { "Sunday","Monday","Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            ViewBag.Name = new SelectList(WeekDays);
            var id = User.Identity.GetUserId();
            var usernow = db.Employees.Include(e => e.ApplicationUser).FirstOrDefault(e => e.ApplicationId == id);
            int zipcode = usernow.ZipCode;
            
            PickByDayViewModels models = new PickByDayViewModels();
            var customers = db.Customers.Include(c => c.ApplicationUser).Where(c=>c.ZipCode == zipcode).ToList();
            models.customers = customers;
            return View(models);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PickByDay(PickByDayViewModels model)
        {
            PickByDayViewModels models = new PickByDayViewModels();
            string datenow = System.DateTime.Now.ToString("MM/dd/yyyy");
            var id = User.Identity.GetUserId();
            var usernow = db.Employees.Include(e => e.ApplicationUser).FirstOrDefault(e => e.ApplicationId == id);
            int zipcode = usernow.ZipCode;
            var customers = db.Customers.Include(e => e.ApplicationUser).Where(e => e.PickUpDay == model.Day || e.ExtraPickUpDate == datenow && e.ZipCode == zipcode).ToList();
            models.customers = customers;
            List<string> WeekDays = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            ViewBag.Name = new SelectList(WeekDays);
         
            return View(models);
        }
        // GET: Employees/Details/5
        public ActionResult Details()
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
            var employee = db.Employees.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Employee employee)
        {
            try
            {
                // TODO: Add update logic here
                var employeeedit = db.Employees.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
                employeeedit.ApplicationUser.UserName = employee.ApplicationUser.UserName;
                employeeedit.ApplicationUser.Email = employee.ApplicationUser.Email;
                employeeedit.FirstName = employee.FirstName;
                employeeedit.LastName = employee.LastName;
                employeeedit.ZipCode = employee.ZipCode;
                
                db.SaveChanges();
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
        // GET: Employees/Edit/5
        public ActionResult PickUpConfirmation(int id)
        {
            var customer = db.Customers.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
            return View(customer);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        public ActionResult PickUpConfirmation(int id, Customer customer)
        {
            try
            {
                // TODO: Add update logic here
                var customerpickup = db.Customers.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
                customerpickup.PickupConfirmation = customer.PickupConfirmation;
                if (customer.PickupConfirmation == true)
                {
                    customerpickup.Balance += 10.50;
                }
                db.SaveChanges();
                return RedirectToAction("TodaysPickUp");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult AddressMap(int id)
        {
            try
            {
                MapViewModel model = new MapViewModel();
                var customer = db.Customers.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
                model.customer = customer;
                model.Key = GoogleMapAPIKey.MapKey;
                model.Address = customer.StreetAddress +" "+ customer.City +" "+ customer.State;
                string key;
                string address;

                key = GoogleMapAPIKey.MapKey;
                address = customer.StreetAddress + ", " + customer.City + ", " + customer.State;
                string requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?key={1}&address={0}&sensor=false", Uri.EscapeDataString(address), key);

                WebRequest request = WebRequest.Create(requestUri);
                WebResponse response = request.GetResponse();
                XDocument xdoc = XDocument.Load(response.GetResponseStream());

                XElement result = xdoc.Element("GeocodeResponse").Element("result");
                XElement locationElement = result.Element("geometry").Element("location");
                XElement lat = locationElement.Element("lat");
                XElement lng = locationElement.Element("lng");
                var latx = lat.Value;
                var lngx = lng.Value;
                string map = string.Format("https://maps.googleapis.com/maps/api/staticmap?center={0}&zoom13&size=600x400&maptype=roadmap&markers=color:black%7C{2},{3}&key={1}", Uri.EscapeDataString(address), key, latx, lngx);
                ViewBag.map = map;
                return View(model);
                
            }
            catch
            {

                return RedirectToAction("PickByDay");
            }
        }
    }
}
