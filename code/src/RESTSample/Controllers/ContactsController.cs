using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RESTSample.Models;

namespace RESTSample.Controllers
{
    public class ContactsController : Controller
    {
        [HttpGet, Authorize(Roles="ViEw")]
        public ActionResult Index()
        {
            List<Contact> contacts = new List<Contact>();

            contacts.Add(new Contact { FirstName = "Don", LastName = "Juan" });
            contacts.Add(new Contact { FirstName = "John", LastName = "Doe" });
            contacts.Add(new Contact { FirstName = "Jane", LastName = "Doe" });

            return Json(contacts, JsonRequestBehavior.AllowGet);
        }

    }
}
