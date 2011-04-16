using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Fluent;
using SharpOAuthProvider.Domain.Repository;
using Microsoft.Practices.ServiceLocation;
using SharpOAuthProvider.Domain;
using SharpOAuth2.Provider.TokenEndpoint;
namespace SharpOAuth2.ProviderSite.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Redirect("http://localhost:15075");
        }

        [HttpPost]
        public void Token()
        {
            TokenResponse response = ControllerContext.HttpContext.Request.ToTokenContext()
                .GrantAuthorizationToken()
                .CreateTokenResponse();

            ControllerContext.HttpContext.Response.WriteTokenResponse(response);
        }

        [Authorize]
        public ActionResult Authorize()
        {
            IAuthorizationContext authContext = ControllerContext.HttpContext.Request.ToAuthorizationContext();
            Session["context"] = authContext;
            IClientRepository clientRepo = ServiceLocator.Current.GetInstance<IClientRepository>();

            Client client = clientRepo.FindClient(authContext.Client.ClientId);
            ViewBag.ClientName = client.Name;
            return View("Authorize", authContext);
        }

        [Authorize]
        public ActionResult HandleAuthorization( string authButton)
        {
            IAuthorizationContext ctx = (IAuthorizationContext)Session["context"];
            IAuthorizationProvider provider = ServiceLocator.Current.GetInstance<IAuthorizationProvider>();

            bool isAuthorized = (authButton == "GRANT");

            Uri response = ctx.SetApproval(isAuthorized)
                .SetResourceOwner(User.Identity.Name)
                .CreateAuthorizationGrant()
                .CreateAuthorizationResponse();

            return Redirect(response.AbsoluteUri);
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
