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
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.Framework;

namespace SharpOAuth2.Provider.Fluent
{
    public static class FluentAuthorizationContext
    {
        readonly static ILog Log = LogManager.GetCurrentClassLogger();
        readonly static object Lck = new object();
        private static IContextBuilder<IAuthorizationContext> GetBuilder()
        {
            IContextBuilder<IAuthorizationContext> builder;
            try
            {
                builder = ServiceLocator.Current.GetInstance<IContextBuilder<IAuthorizationContext>>();
            }
            catch (Exception ex)
            {
                Log.Info("Faild to inject IContextBuilder<IAuthorizationContext>", ex);
                builder = new AuthorizationContextBuilder();
            }
            return builder;
        }

        private static IAuthorizationProvider _provider;
        private static IAuthorizationProvider GetProvider()
        {
            if (_provider != null)
                return _provider;
            try
            {
                lock (Lck)
                {
                    _provider = ServiceLocator.Current.GetInstance<IAuthorizationProvider>();
                }
                return _provider;
            }
            catch (Exception x)
            {
                Log.Error("Failed to inject the AuthorizationProvider", x);
                throw;
            }
        }

        static IAuthorizationResponseBuilder _responseBuilder;
        private static IAuthorizationResponseBuilder GetResponseBuilder()
        {
            if (_responseBuilder != null)
                return _responseBuilder;

            lock (Lck)
            {
                try
                {
                    _responseBuilder = ServiceLocator.Current.GetInstance<IAuthorizationResponseBuilder>();
                }
                catch (Exception x)
                {
                    Log.Info("Failed to inject IAuthorizationResponseBuilder", x);
                    _responseBuilder = new AuthorizationResponseBuilder();
                }
            }
            return _responseBuilder;
        }
        public static IAuthorizationContext ToAuthorizationContext(this HttpRequest reqeust)
        {
            return ToAuthorizationContext(new HttpRequestWrapper(reqeust));
        }
        public static IAuthorizationContext ToAuthorizationContext(this HttpRequestBase request)
        {
            IContextBuilder<IAuthorizationContext> builder = GetBuilder();

            return builder.FromHttpRequest(request);
        }

        public static IAuthorizationContext ToAuthorizationContext(this Uri uri)
        {
            IContextBuilder<IAuthorizationContext> builder = GetBuilder();

            return builder.FromUri(uri);
        }

        public static IAuthorizationContext SetResourceOwner( this IAuthorizationContext context, string resourceOwnerId)
        {
            context.ResourceOwnerId = resourceOwnerId;
            return context;
        }

        public static IAuthorizationContext SetApproval(this IAuthorizationContext context, bool isApproved)
        {
            context.IsApproved = isApproved;
            return context;
        }

        public static IAuthorizationContext CreateAuthorizationGrant(this IAuthorizationContext context)
        {
            GetProvider().CreateAuthorizationGrant(context);
            return context;
        }

        public static Uri CreateAuthorizationResponse(this IAuthorizationContext context)
        {
            return GetResponseBuilder().CreateResponse(context);
        }

        public static bool IsAccessApproved(this IAuthorizationContext context)
        {
            return GetProvider().IsAccessApproved(context);
        }


    }
}
