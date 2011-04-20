#region License
/* The MIT License
 * 
 * Copyright (c) 2011 Geoff Horsey
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Globalization;
using SharpOAuth2.Provider.Utility;

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
                throw new HttpException(405, string.Format(CultureInfo.CurrentUICulture, AuthorizationEndpointResources.InvalidRequestMethod,
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

        

        private IAuthorizationContext CreateContext(NameValueCollection values)
        {
            AuthorizationContext context = new AuthorizationContext();
            context.Client = CreateClient(values[Parameters.ClientId], values[Parameters.ClientSecret]);
            context.RedirectUri = ContextBuilderHelpers.CreateRedirectUri(values[Parameters.RedirectUri]);
            context.ResponseType = values[Parameters.ResponseType];
            context.State = values[Parameters.State];
            context.Scope = ContextBuilderHelpers.CreateScope(values[Parameters.Scope]);

            return context;
        }
    }
}
