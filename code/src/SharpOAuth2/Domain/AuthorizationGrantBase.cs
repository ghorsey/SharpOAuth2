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
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Utility;

namespace SharpOAuth2.Provider.Domain
{
    public class AuthorizationGrantBase : IToken
    {
        #region IToken Members

        public virtual string Token { get; set; }
        public virtual string TokenType { get; set; }
        public virtual int ExpiresIn { get; set; }
        public virtual string RefreshToken { get; set; }
        public virtual string[] Scope { get; set; }
        public virtual long IssuedOn { get; private set; }
        public virtual bool IsApproved { get; set; }
        public virtual ClientBase Client { get; set; }
        public virtual Uri RedirectUri { get; set; }
        public virtual IDictionary<string, object> Parameters { get; private set; }
        #endregion

        public AuthorizationGrantBase()
        {
            IssuedOn = DateTime.Now.ToEpoch();
            Parameters = new Dictionary<string, object>();
        }

        #region IToken Members


        public IDictionary<string, object> ToResponseValues()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters[SharpOAuth2.Provider.Framework.Parameters.AuthroizationCode] = Token;


            foreach (var item in Parameters)
                parameters.Add(item.Key, item.Value);

            return parameters;
        }

        #endregion
    }
}
