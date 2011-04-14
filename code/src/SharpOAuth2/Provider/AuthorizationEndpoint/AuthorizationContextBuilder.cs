using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SharpOAuth2.Globalization;
using System.Globalization;
using System.Collections.Specialized;
using System.Net;
using System.IO;

namespace SharpOAuth2.Provider.AuthorizationEndpoint
{
    public class AuthorizationContextBuilder : IContextBuilder<IAuthorizationContext>
    {
        #region IAuthorizationContextBuilder Members

        public IAuthorizationContext FromUri(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException("url");

            Uri uri = new Uri(url, UriKind.Absolute);

            return FromUri(uri);
        }

        public IAuthorizationContext FromUri(Uri uri)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            NameValueCollection q = HttpUtility.ParseQueryString(uri.Query);
            return CreateContext(q);
        }


        public IAuthorizationContext FromHttpRequest(System.Web.HttpRequestBase request)
        {
            NameValueCollection values;

            if (request.HttpMethod.ToUpperInvariant() == "GET")
                values = request.QueryString;
            else if (request.HttpMethod.ToUpperInvariant() == "POST")
                values = request.Form;
            else
                throw new HttpException(405, string.Format(CultureInfo.CurrentUICulture, AuthorizationResources.InvalidRequestMethod,
                    request.HttpMethod));

            return CreateContext(values);
        }

        #endregion

        private IClient CreateClient(string clientId, string clientSecret)
        {
            return new ClientBase
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            };
        }
        private Uri CreateRedirectUri(string uri)
        {
            if (string.IsNullOrWhiteSpace(uri)) return null;
            return new Uri(uri, UriKind.Absolute);
        }

        private string[] CreateScope(string scope)
        {
            if (string.IsNullOrWhiteSpace(scope)) return new string[0];

            return scope.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private IAuthorizationContext CreateContext(NameValueCollection values)
        {
            AuthorizationContext context = new AuthorizationContext();
            context.Client = CreateClient(values[Parameters.ClientId], values[Parameters.ClientSecret]);
            context.RedirectUri = CreateRedirectUri(values[Parameters.RedirectUri]);
            context.ResponseType = values[Parameters.ResponseType];
            context.State = values[Parameters.State];
            context.Scope = CreateScope(values[Parameters.Scope]);

            return context;
        }
    }
}
