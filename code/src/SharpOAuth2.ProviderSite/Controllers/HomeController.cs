using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Fluent;
using SharpOAuth2.Mvc;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.ResourceEndpoint;
using SharpOAuthProvider.Domain;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuth2.Provider;
using Newtonsoft.Json;
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
        public ActionResult Token()
        {
            TokenResponse response = ControllerContext.HttpContext.Request.ToTokenContext()
                .GrantAuthorizationToken()
                .CreateTokenResponse();

            return new TokenResponseActionResult(response);
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
                FormsAuthentication.SetAuthCookie(username, false);
                return Redirect(FormsAuthentication.GetRedirectUrl(username, false));
            }
            else
                return View("LogOn");
        }


        [HttpGet]
        public ActionResult ViewResourceOwnerData()
        {
            IResourceContext context = new ResourceContextBuilder().FromHttpRequest(ControllerContext.HttpContext.Request);

            IResourceProvider provider = ServiceLocator.Current.GetInstance<IResourceProvider>();



            object[] list = new object[]
            {
                new { FirstName = "Geoff", LastName="Horsey"},
                new { FirstName = "John", LastName = "Doe"},
                new { FirstName = "Jane", LastNmae ="Doe"}
            };

            try
            {
                provider.AccessProtectedResource(context);
                provider.ValidateScope(context, new string[] { "view" });

                return Json(list, JsonRequestBehavior.AllowGet);
                
            }
            catch(OAuthErrorResponseException<IResourceProvider> x)
            {
                throw new HttpException(x.HttpStatusCode, x.Message);
            }

        }

    }
}
