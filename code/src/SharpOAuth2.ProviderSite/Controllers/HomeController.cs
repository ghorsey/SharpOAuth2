using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SharpOAuth2.Provider.Authorization;
using SharpOAuth2.Fluent;
using SharpOAuthProvider.Domain.Repository;
using Microsoft.Practices.ServiceLocation;
using SharpOAuthProvider.Domain;
namespace SharpOAuth2.ProviderSite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [Authorize]
        public ActionResult Authorize()
        {
            IAuthorizationContext authContext = ControllerContext.HttpContext.Request.ToAuthorizationContext();
            Session["context"] = authContext;
            IClientRepository clientRepo = ServiceLocator.Current.GetInstance<IClientRepository>();

            Client client = clientRepo.LoadClient(authContext.Client.ClientId);
            ViewBag.ClientName = client.Name;
            return View("Authorize", authContext);
        }

        [Authorize]
        public ActionResult HandleAuthorization( string authButton)
        {
            IAuthorizationContext ctx = (IAuthorizationContext)Session["context"];
            IAuthorizationProvider provider = ServiceLocator.Current.GetInstance<IAuthorizationProvider>();

            bool isAuthorized = (authButton == "GRANT");

            ctx.SetApproval(isAuthorized)
                .SetResourceOwner(User.Identity.Name)
                .CreateAuthorizationGrant();

            return null;
        }

        [HttpGet]
        public ActionResult LogOn()
        {
            return View("LogOn");
        }

        [HttpPost]
        public ActionResult LogOn(string username, string password)
        {
            if (FormsAuthentication.Authenticate(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, true);
                return Redirect(FormsAuthentication.GetRedirectUrl(username, true));
            }
            else
                return View("LogOn");
        }
    }
}
