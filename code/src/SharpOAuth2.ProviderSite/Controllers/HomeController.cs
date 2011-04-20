using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.Fluent;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Mvc;
using SharpOAuth2.Provider.ResourceEndpoint;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuthProvider.Domain;
using SharpOAuthProvider.Domain.Repository;

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
                .GrantAccessToken()
                .CreateTokenResponse();

            return new TokenResponseActionResult(response);
        }

        [Authorize]
        public ActionResult Authorize()
        {
            IAuthorizationContext authContext = ControllerContext.HttpContext.Request.ToAuthorizationContext()
                .SetResourceOwner(User.Identity.Name);


            if (authContext.IsAccessApproved())
                return Redirect(authContext.SetApproval(true)
                    .CreateAuthorizationGrant()
                    .CreateAuthorizationResponse().AbsoluteUri);

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
            //IAuthorizationProvider provider = ServiceLocator.Current.GetInstance<IAuthorizationProvider>();

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
