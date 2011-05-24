using System;
using System.Security.Principal;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.ResourceEndpoint;

namespace SharpOAuth2.Provider.ResourceAuthorizationHttpModule
{
    public class OAuthAuthorizeModule : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.AuthorizeRequest += new EventHandler(Application_AuthenticateRequest);
        }

        #endregion

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            IResourceProvider provider = ServiceLocator.Current.GetInstance<IResourceProvider>();

            IResourceContext context = new ResourceContextBuilder().FromHttpRequest(new HttpRequestWrapper(HttpContext.Current.Request));

            provider.AccessProtectedResource(context);

            TokenPrincipal principal = new TokenPrincipal(new GenericIdentity(context.Token.Token, "OAuth"), context.Token.Scope, context.Token);


            HttpContext.Current.User = principal;

        }

    }
}