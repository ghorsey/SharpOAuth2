using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpOAuth2.ClientSite.Models.Home;
using System.Net;
using System.Text;
using System.IO;

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

        public ActionResult Callback(string code, string error, string error_description)
        {
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
                request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("mykeys")));
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                Stream reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();


                WebResponse response = request.GetResponse();
                
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    accessToken = sr.ReadToEnd();
                }
            }

            return View("Callback", new CallbackModel(accessToken, !string.IsNullOrWhiteSpace(error), error, error_description));
        }
    }
}
