using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using Common.Logging;
using SharpOAuth2.Provider;

namespace SharpOAuth2.Fluent
{
    public static class FluentAuthorizationContext
    {
        readonly static ILog Log = LogManager.GetCurrentClassLogger();

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
                lock (_provider)
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

            lock (_responseBuilder)
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
