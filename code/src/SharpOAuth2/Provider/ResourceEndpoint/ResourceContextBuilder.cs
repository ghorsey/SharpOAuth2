using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SharpOAuth2.Provider.ResourceEndpoint
{
    public class ResourceContextBuilder : IContextBuilder<IResourceContext>
    {

        #region IContextBuilder<IResourceContext> Members

        public IResourceContext FromUri(string url)
        {
            return FromUri(new Uri(url, UriKind.Absolute));
        }

        public IResourceContext FromUri(Uri uri)
        {
            ResourceContext context = new ResourceContext();

            context.QueryString = HttpUtility.ParseQueryString(uri.Query);

            return context;
        }

        public IResourceContext FromHttpRequest(System.Web.HttpRequestBase request)
        {
            ResourceContext context = new ResourceContext();
            context.Form = request.Form;
            context.QueryString = request.QueryString;
            context.Headers = request.Headers;

            return context;
        }

        #endregion
    }
}
