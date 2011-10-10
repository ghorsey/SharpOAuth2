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
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Domain;
using System.Collections.Specialized;

namespace SharpOAuth2.Provider.AuthorizationEndpoint
{
    public class AuthorizationContext : IAuthorizationContext
    {
        #region IAuthorizationContext Members
        public AuthorizationGrantBase AuthorizationGrant { get; set; }
        public IClient Client { get; set; }
        public string ResponseType { get; set; }
        public Uri RedirectUri { get; set; }
        public string[] Scope { get; set; }
        public string State { get; set; }
        public ErrorResponse Error { get; set; }
        public IToken Token { get; set; }
        public bool IsApproved { get; set; }
        public string ResourceOwnerUsername { get; set; }
        public NameValueCollection Headers { get; set; }
        public NameValueCollection Form { get; set; }
        public NameValueCollection QueryString { get; set; }
        #endregion

    }
}
