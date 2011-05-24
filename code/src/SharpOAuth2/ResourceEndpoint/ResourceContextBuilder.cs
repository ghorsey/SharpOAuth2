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
using System.Web;
using SharpOAuth2.Provider.Framework;

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
