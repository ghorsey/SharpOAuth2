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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using SharpOAuth2.Framework;
using SharpOAuth2.Framework.Utility;

namespace SharpOAuth2.Client.AuthorizationEndpoint
{
    public class AuthorizationRequest
    {
        public string ResponseType { get; set; }
        public string[] Scope { get; set; }
        public Uri RedirectUri { get; set; }
        public Uri Endpoint { get; set; }
        public string ClientId { get; set; }
        public string Method { get; set; }
        public string ToAbsoluteUri()
        {
            UriBuilder builder = new UriBuilder(Endpoint);
            NameValueCollection components = new NameValueCollection();
            components[Parameters.ResponseType] = ResponseType;
            components[Parameters.ClientId] = ClientId;

            if (Scope != null && Scope.Length > 0)
                components[Parameters.Scope] = string.Join(" ", Scope);

            components[Parameters.RedirectUri] = RedirectUri.AbsoluteUri;

            builder.Query = UriHelper.ReconstructQueryString(components);

            return builder.Uri.AbsoluteUri;
        }
    }
}
