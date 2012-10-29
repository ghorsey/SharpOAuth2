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
using SharpOAuth2.Framework;
using OAParameters = SharpOAuth2.Framework.Parameters;
namespace SharpOAuth2.Client
{
    public class AccessToken : IToken
    {
        public AccessToken(IDictionary<string, string> source)
        {
            Token = (string)SafeGet(OAParameters.AccessToken, source) ?? string.Empty;
            TokenType = (string)SafeGet(OAParameters.AccessTokenType, source) ?? string.Empty;
            ExpiresIn = Convert.ToInt32(SafeGet(OAParameters.AccessTokenExpiresIn, source));
            RefreshToken = (string)SafeGet(OAParameters.RefreshToken, source) ?? string.Empty;
            if (!string.IsNullOrWhiteSpace((string)SafeGet(OAParameters.Scope, source) ?? string.Empty))
                Scope = ((string)SafeGet(OAParameters.Scope, source)).Split(' ');
            Parameters = source;
        }

        private object SafeGet(string key, IDictionary<string, string> source)
        {
            if (!source.ContainsKey(key))
                return null;

            return source[key];
        }
        #region IToken Members

        public string Token { get; set; }
        public string TokenType{ get; set; }
        public int ExpiresIn{ get; set; }
        public string RefreshToken{ get; set; }
        public string[] Scope{ get; set; }
		public long IssuedOn { get; private set; }

        public IDictionary<string, string> Parameters{ get; private set; }
        #endregion
    }
}
