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
using System.Collections.Specialized;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using SharpOAuth2.Framework;
using SharpOAuth2.Framework.Utility;

namespace SharpOAuth2.Client.TokenEndpoint
{
    public class TokenRequest
    {
        public Uri TokenEndpoint{ get; private set; }

        public TokenRequest(Uri tokenEndpoint)
        {
            TokenEndpoint = tokenEndpoint;
        }

        public IToken RefreshAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            AssertClientPresent(clientId, clientSecret);
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentNullException("refreshToken");

            NameValueCollection components = MakeCommonComponents(clientId, clientSecret);
            components[Parameters.RefreshToken] = refreshToken;
            components[Parameters.GrantType] = Parameters.GrantTypeValues.RefreshToken;

            return FetchToken(components);
        }

        public IToken ExchangeClientCredentials(string clientId, string clientSecret)
        {
            AssertClientPresent(clientSecret, clientSecret);

            NameValueCollection components = MakeCommonComponents(clientId, clientSecret);
            components[Parameters.GrantType] = Parameters.GrantTypeValues.ClientCredentials;

            return FetchToken(components);
        }
        public IToken ExchangeResourceOwnerCredentials(string clientId, string clientSecret, string resourceOwnerUsername, string resourceOwnerPassword)
        {
            AssertClientPresent(clientId, clientSecret);
            if (string.IsNullOrWhiteSpace(resourceOwnerUsername))
                throw new ArgumentNullException("resourceOwnerUsername");
            if (string.IsNullOrWhiteSpace(resourceOwnerPassword))
                throw new ArgumentNullException("resourceOwnerPassword");

            NameValueCollection components = MakeCommonComponents(clientId, clientSecret);
            components[Parameters.GrantType] = Parameters.GrantTypeValues.Password;
            components[Parameters.ResourceOwnerUsername] = resourceOwnerUsername;
            components[Parameters.ResourceOwnerPassword] = resourceOwnerPassword;

            return FetchToken(components);
        }
        public IToken ExchangeAuthorizationGrant(string clientId, string clientSecret, string code, Uri redirectUri)
        {
            AssertClientPresent(clientId, clientSecret);
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException("code");
            if (redirectUri == null)
                throw new ArgumentNullException("redirectUri");

            NameValueCollection components = MakeCommonComponents(clientId, clientSecret);
            components[Parameters.GrantType] = Parameters.GrantTypeValues.AuthorizationCode;
            components[Parameters.AuthroizationCode] = code;
            components[Parameters.RedirectUri] = redirectUri.AbsoluteUri;

            return FetchToken(components);
        }

        private static NameValueCollection MakeCommonComponents(string clientId, string clientSecret)
        {
            NameValueCollection components = new NameValueCollection();
            components[Parameters.ClientId] = clientId;
            components[Parameters.ClientSecret] = clientSecret;
            return components;
        }

        private static void AssertClientPresent(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentNullException("clientId");
            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentNullException("clientSecret");
        }

        private IToken FetchToken(NameValueCollection components)
        {
            string postData = UriHelper.ReconstructQueryString(components);
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(postData);

            WebRequest request = WebRequest.Create(TokenEndpoint);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            Stream reqStream = request.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();


            try
            {
                WebResponse response = request.GetResponse();

                string accessToken = string.Empty;

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    accessToken = sr.ReadToEnd();

                Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(accessToken);

                return new AccessToken(dictionary);
            } catch( WebException)
            {
                throw;
            }
        }
    }
}
