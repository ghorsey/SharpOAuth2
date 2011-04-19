using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpOAuth2.ClientSite.Models.Home;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace SharpOAuth2.ClientSite.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View("Index");
        }
        public ActionResult ResourceOwnerPassword(string username, string password)
        {
            //TODO: This is all ugly and need refactoring when I build the oauth client routines
            string accessToken = "";

            StringBuilder postData = new StringBuilder();

            postData.Append("grant_type=password");
            postData.Append("&client_id=12345");
            postData.Append("&client_secret=secret");
            postData.AppendFormat("&username={0}", Uri.EscapeDataString(username));
            postData.AppendFormat("&password={0}", Uri.EscapeDataString(password));

            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(postData.ToString());

            WebRequest request = WebRequest.Create("http://localhost:15079/Home/Token");
            
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();

            WebResponse response = request.GetResponse();

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                accessToken = sr.ReadToEnd();

            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(accessToken);
            Session["Token"] = dictionary;

            return RedirectToAction("ViewResourceData");


        }
        public ActionResult Callback(string code, string error, string error_description)
        {
            //TODO: This is all ugly and need refactoring when I build the oauth client routines
            string accessToken = "";
            if (string.IsNullOrWhiteSpace(error))
            {
                StringBuilder postData = new StringBuilder();

                postData.Append("grant_type=authorization_code");
                postData.Append("&client_id=12345");
                postData.Append("&client_secret=secret");
                postData.Append("&code=");
                postData.Append(Uri.EscapeDataString(code));
                postData.Append("&redirect_uri=");
                postData.Append(Uri.EscapeDataString("http://localhost:15075/Home/Callback"));
                byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(postData.ToString());

                WebRequest request = WebRequest.Create("http://localhost:15079/Home/Token");

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();


                WebResponse response = request.GetResponse();
                
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    accessToken = sr.ReadToEnd();

                Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(accessToken);
                Session["Token"] = dictionary;

                return RedirectToAction("ViewResourceData");
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                return View("Callback", new CallbackModel(accessToken, !string.IsNullOrWhiteSpace(error), error, error_description));
            }
            else
            {
                return RedirectToAction("ViewData");
            }
        }

        [HttpGet]
        public ActionResult ViewResourceData()
        {
            IDictionary<string, object> token = (IDictionary<string, object>)Session["Token"];
            WebRequest request = WebRequest.Create("http://localhost:15079/Home/ViewResourceOwnerData");
            request.Method = "GET";
            request.Headers["Authorization"] = "Bearer " + token["access_token"];

            WebResponse response = request.GetResponse();

            string result = string.Empty;
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                result = sr.ReadToEnd();

            return View("ViewResourceData", (object)result);
        }
    }
}
