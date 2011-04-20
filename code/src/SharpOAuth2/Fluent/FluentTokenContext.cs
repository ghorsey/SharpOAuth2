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
using Common.Logging;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.TokenEndpoint;

namespace SharpOAuth2.Provider.Fluent
{
    public static class FluentTokenContext
    {
        readonly static ILog Log = LogManager.GetCurrentClassLogger();

        private static IContextBuilder<ITokenContext> GetContextBuilder()
        {
            IContextBuilder<ITokenContext> builder;
            try
            {
                builder = ServiceLocator.Current.GetInstance<IContextBuilder<ITokenContext>>();
            }
            catch (Exception ex)
            {
                Log.Info("Failed to inject IContextBuilder<ITokenContext>", ex);
                builder = new TokenContextBuilder();
            }

            return builder;
        }

        private static ITokenProvider GetProvider()
        {
            try
            {
                return ServiceLocator.Current.GetInstance<ITokenProvider>();
            }
            catch (Exception x)
            {
                Log.Error("Failed to inject ITokenProvider", x);
                throw;
            }
        }

        private static ITokenResponseBuilder GetResponseBuilder()
        {
            ITokenResponseBuilder builder;

            try
            {
                builder = ServiceLocator.Current.GetInstance<ITokenResponseBuilder>();
            }
            catch (Exception x)
            {
                Log.Info("Failed to inject ITokenResponseBuilder, using default", x);
                builder = new TokenResponseBuilder();
            }

            return builder;
        }

        public static ITokenContext ToTokenContext(this HttpRequest request)
        {
            return ToTokenContext(new HttpRequestWrapper(request));
        }
        public static ITokenContext ToTokenContext(this HttpRequestBase request)
        {
            IContextBuilder<ITokenContext> builder = GetContextBuilder();
            return builder.FromHttpRequest(request);
        }

        public static ITokenContext GrantAccessToken(this ITokenContext context)
        {
            ITokenProvider provider = GetProvider();

            provider.GrantAccessToken(context);
            return context;
        }

        public static TokenResponse CreateTokenResponse(this ITokenContext context)
        {
            ITokenResponseBuilder builder = GetResponseBuilder();
            return builder.CreateResponse(context);
        }

        public static void WriteTokenResponse(this HttpResponse response, TokenResponse tokenResponse)
        {
            WriteTokenResponse(new HttpResponseWrapper(response), tokenResponse);
        }
        public static void WriteTokenResponse(this HttpResponseBase response, TokenResponse tokenResponse)
        {
            TokenResponseWriter writer = new TokenResponseWriter(response);

            writer.WriteResponse(tokenResponse);
        }
    }
}
