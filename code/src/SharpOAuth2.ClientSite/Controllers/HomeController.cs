using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using SharpOAuth2.Client.Authorization;
using SharpOAuth2.ClientSite.Models.Home;
using SharpOAuth2.Client.Token;
using SharpOAuth2.Client;
using SharpOAuth2.Framework;

namespace SharpOAuth2.ClientSite.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View("Index");
        }

        #region Authorization Endpoint calls
        [HttpGet]
        public ActionResult ImplicitFlow()
        {
            AuthorizationRequest request = new AuthorizationRequest
            {
                ClientId = "12345",
                ResponseType = "token",
                Scope = new string[] { "view", "edit" },
                Endpoint = new Uri("http://localhost:15079/Home/Authorize"),
                RedirectUri = new Uri("http://localhost:15075/Home/Callback")
            };

            return Redirect(request.ToAbsoluteUri());
        }
        [HttpGet]
        public ActionResult RedirectFlow()
        {
            AuthorizationRequest request = new AuthorizationRequest
            {
                ClientId = "12345",
                ResponseType = "code",
                RedirectUri = new Uri("http://localhost:15075/Home/Callback"),
                Endpoint = new Uri("http://localhost:15079/Home/Authorize"),
                Scope = new string[] { "view", "edit" }
            };

            return Redirect(request.ToAbsoluteUri());
        }

        #endregion

        #region Token Endpoint calls
        [HttpGet]
        public ActionResult ClientCredentials()
        {
            //Session session = new Session(new Uri("http://localhost:15079/Home/Token"));
            //IToken token = session.ClientCredentials("refresh")
            //    .ExchangeForToken();
            TokenSession session = new TokenSession(new Uri("http://localhost:15079/Home/Token"));
            IToken token = session.ExchangeClientCredentials("12345", "secret");

            return View("ClientCredentials", token);
        }

        [HttpGet]
        public ActionResult RefreshToken()
        {
            //Session session = new Session(new Uri("http://localhost:15079/Home/Token"));
            //IToken token = session.RefreshToken("refresh")
            //    .SetClient("12345", "secret")
            //    .ExchangeForToken();
         
            TokenSession session = new TokenSession(new Uri("http://localhost:15079/Home/Token"));

            Session["Token"] = session.RefreshAccessToken("12345", "secret", "refresh");

            return RedirectToAction("ViewResourceData");
        }

        [HttpPost]
        public ActionResult ResourceOwnerPassword(string username, string password)
        {
            //IToken token = session.ResourceOwnerPasswordCredentials(username, password)
            //    .SetClient("12345", "secret")
            //    .ExchangeForToken();
            TokenSession session = new TokenSession(new Uri("http://localhost:15079/Home/Token"));

            Session["Token"] = session.ExchangeResourceOwnerCredentials("12345", "secret", username, password);

            return RedirectToAction("ViewResourceData");
        }
        #endregion

        public ActionResult Callback(string code, string error, string error_description)
        {
            if (string.IsNullOrWhiteSpace(error) && !string.IsNullOrEmpty(code))
            {
                TokenSession session = new TokenSession(new Uri("http://localhost:15079/Home/Token"));
                IToken token = session.ExchangeAuthorizationGrant("12345", "secret", code, new Uri("http://localhost:15075/Home/Callback"));
                //Session session = new Session(new Uri("http://localhost:15079/Home/Token"));
                //IToken token = session.ExchangeAuthorizationGrant(code)
                //    .SetClient("12345", "secret")
                //    .ExchangeForToken();

                Session["Token"] = token;

                return RedirectToAction("ViewResourceData");
            }

            if (!string.IsNullOrWhiteSpace(error) || string.IsNullOrWhiteSpace(code))
            {
                return View("Callback", new CallbackModel(!string.IsNullOrWhiteSpace(error), error, error_description));
            }
            else
            {
                return RedirectToAction("ViewData");
            }
        }

        [HttpGet]
        public ActionResult ViewResourceData()
        {
            IToken token = (IToken)Session["Token"];
            WebRequest request = WebRequest.Create("http://localhost:15079/Home/ViewResourceOwnerData");
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + token.Token;

            WebResponse response = request.GetResponse();

            string result = string.Empty;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                result = sr.ReadToEnd();

            return View("ViewResourceData", (object)result);
        }
    }
}
