using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using Trashcollector.Models;
using System.Net;
using System.Xml.Linq;

namespace Trashcollector.Controllers
{
    public class CustomersController : Controller
    {
        ApplicationDbContext db;
        public CustomersController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.ApplicationUser).ToList();
            return View(customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details()
        {
            var id = User.Identity.GetUserId();
            var customer = db.Customers.Include(c=>c.ApplicationUser).SingleOrDefault(c => c.ApplicationId == id);            
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            Customer customer = new Customer();
            return View(customer);
        }

        // POST: Customers/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                // TODO: Add insert logic here
                customer.ApplicationId = User.Identity.GetUserId();
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Details" , "Customers");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            var customer = db.Customers.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Customer customer)
        {
            try
            {
                // TODO: Add update logic here
                var customeredit = db.Customers.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
                customeredit.ApplicationUser.UserName = customer.ApplicationUser.UserName;
                customeredit.ApplicationUser.Email = customer.ApplicationUser.Email;
                customeredit.FirstName = customer.FirstName;
                customeredit.LastName = customer.LastName;
                customeredit.StreetAddress = customer.StreetAddress;
                customeredit.City = customer.City;
                customeredit.State = customer.State;
                customeredit.ZipCode = customer.ZipCode;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Edit/5
        public ActionResult EditPickUp()
        {
            var id = User.Identity.GetUserId();
            var customer = db.Customers.Include(c => c.ApplicationUser).SingleOrDefault(c => c.ApplicationId == id);
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        public ActionResult EditPickUp(Customer customer)
        {
            try
            {
                // TODO: Add update logic here
                var id = User.Identity.GetUserId();
                var customeredit = db.Customers.Include(c => c.ApplicationUser).SingleOrDefault(c => c.ApplicationId == id);

                customeredit.PickUpDay = customer.PickUpDay;
                customeredit.ExtraPickUpDate = customer.ExtraPickUpDate;
                customeredit.SuspendStart = customer.SuspendStart;
                customeredit.SuspendEnd = customer.SuspendEnd;
                customeredit.PickupConfirmation = customer.PickupConfirmation;
                customeredit.Balance = customer.Balance;               
                db.SaveChanges();

                return RedirectToAction("Details", "Customers");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            var customer = db.Customers.Include(c => c.ApplicationUser).SingleOrDefault(c => c.Id == id);
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Customer customer)
        {
            try
            {
                // TODO: Add delete logic here
                customer = db.Customers.Include(c=>c.ApplicationUser).SingleOrDefault(c => c.Id == id);
                var userdelete = db.Users.SingleOrDefault(c => c.Id == customer.ApplicationId);
                customer.ApplicationId = null;
                db.Customers.Remove(customer);
                db.Users.Remove(userdelete);
                db.SaveChanges();
                return RedirectToAction("Index");
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
                model.Address = customer.StreetAddress + " " + customer.City + " " + customer.State;
                string key;
                string address;

                key = GoogleMapAPIKey.MapKey;
                address = customer.StreetAddress + " " + customer.City + " " + customer.State;
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
                string map = string.Format("https://maps.googleapis.com/maps/api//staticmap?center={0}&zoom13&size=600x400&maptype=roadmap&markers=color:black%7c{2},{3}&key={1}", Uri.EscapeDataString(address), key, latx, lngx);
                ViewBag.map = map;
                return View(model);
            }
            catch 
            {

                return RedirectToAction("Index");
            }
        }
    }
}
