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


namespace SharpOAuth2.Framework
{
    internal static class Parameters
    {
        public const string ClientId = "client_id";
        public const string ClientSecret = "client_secret";
        public const string GrantType = "grant_type";
        public const string ResponseType = "response_type";
        public const string RedirectUri = "redirect_uri";
        public const string Scope = "scope";
        public const string State = "state";
        public const string AuthroizationCode = "code";
        public const string RefreshToken = "refresh_token";
        public const string AccessToken = "access_token";
        public const string AccessTokenType = "token_type";
        public const string AccessTokenExpiresIn = "expires_in";
        public const string ResourceOwnerUsername = "username";
        public const string ResourceOwnerPassword = "password";

        public static class AuthorizationResponse
        {
            public const string Code = "code";
        }
        public static class AccessTokenResponse
        {
            public const string AccessToken = "access_token";
            public const string TokenType = "token_type";
            public const string ExpiresIn = "expires_in";
            public const string RefreshToken = "refresh_token";
        }

        public static class AccessTokenTypeValues
        {
            public const string Bearer = "bearer";
            public const string Mac = "mac";

            public static class MacParameters
            {
                public const string Secret = "secret";
                public const string Algorithm = "algorithm";

                public static class AlgorithmValues
                {
                    public const string HmacSha1 = "hmac-sha-1";
                    public const string HmacSha256 = "hmac-sha-256";
                }
            }
        }
        public static class ResponseTypeValues
        {
            public const string AuthorizationCode = "code";
            public const string AccessToken = "token";
        }
        public static class GrantTypeValues
        {
            public const string AuthorizationCode = "authorization_code";
            public const string Password = "password";
            public const string ClientCredentials = "client_credentials";
            public const string RefreshToken = "refresh_token";
        }

        public static class ErrorParameters
        {
            public const string Error = "error";
            public const string ErrorDescription = "error_description";
            public const string ErrorUri = "error_uri";

            public static class ErrorValues
            {
                public const string InvalidToken = "invalid_token";
                public const string InvalidRequest = "invalid_request";
                public const string InvalidClient = "invalid_client";
                public const string InvalidGrant = "invalid_grant";
                public const string UnauthorizedClient = "unauthorized_client";
                public const string AccessDenied = "access_denied";
                public const string UnsupportedResponseType = "unsupported_response_type";
                public const string UnsupportedGrantType = "unsupported_grant_type";
                public const string InvalidScope = "invalid_scope";
                public const string InsufficientScope = "insufficient_scope";
            }
        }

        public const string HttpAuthorizationHeader = "Authorization";
        public static class BearerTokenValues
        {
            public const string BearerTokenRequest = "bearer_token";
        }

    }
}
