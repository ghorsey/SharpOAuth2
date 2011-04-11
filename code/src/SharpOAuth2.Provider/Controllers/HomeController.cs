using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SharpOAuth2.ProviderSite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Authorize()
        {
            return View("Authorize");
        }

        [HttpGet]
        public ActionResult LogOn()
        {
            return View("LogOn");
        }
    }
}
