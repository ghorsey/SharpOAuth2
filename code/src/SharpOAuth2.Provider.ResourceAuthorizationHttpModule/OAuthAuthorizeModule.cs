using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Text;
using System.Web;
using Common.Logging;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.ResourceEndpoint;

namespace SharpOAuth2.Provider.ResourceAuthorizationHttpModule
{
    public class OAuthAuthorizeModule : IHttpModule
    {
        readonly ILog Log = LogManager.GetCurrentClassLogger();

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AuthorizeRequest += new EventHandler(Application_AuthenticateRequest);
        }

        #endregion

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                IResourceProvider provider = ServiceLocator.Current.GetInstance<IResourceProvider>();
                IResourceContext context = new ResourceContextBuilder().FromHttpRequest(new HttpRequestWrapper(HttpContext.Current.Request));
                try
                {
                    provider.AccessProtectedResource(context);
                    TokenPrincipal principal = new TokenPrincipal(new GenericIdentity(context.Token.Token, "OAuth"), context.Token.Scope, context.Token);

                    HttpContext.Current.User = principal;
                }
                catch (OAuthErrorResponseException<IResourceContext> x)
                {
                    Log.Info(m => m("Failed to authorize the token. Error: {0}; Message: {1}", x.Error, x.Message), x);
                    return;
                }
                catch (OAuthFatalException x)
                {
                    StringBuilder report = new StringBuilder();
                    
                    FormatReportHeader(report, "QueryString");
                    FormatReportDictionary(report, context.QueryString);
                    FormatReportHeader(report, "Form");
                    FormatReportDictionary(report, context.Form);
                    FormatReportHeader(report, "Headers");
                    FormatReportDictionary(report, context.Headers);
                    FormatReportHeader(report, "URL");

                    report.AppendLine(HttpContext.Current.Request.Url.ToString());

                    Log.Info(m => m("{0}\r\n{1}", x.Message, report.ToString()), x);
                }
            }
            catch (Exception x)
            {
                Log.Error(x.Message, x);
                throw;
            }
        }

        private void FormatReportHeader(StringBuilder report, string header)
        {
            report.AppendLine(header);
            report.AppendLine("--------------");
        }
        private void FormatReportDictionary(StringBuilder report, NameValueCollection collection)
        {
            foreach (string key in collection.AllKeys)
                report.AppendFormat("{0}={1}\r\n", key, collection[key]);
        }

    }
}