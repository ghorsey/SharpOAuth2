using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Authorization;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using Common.Logging;

namespace SharpOAuth2.Fluent
{
    public static class FluentAuthorizationContext
    {
        readonly static ILog Log = LogManager.GetCurrentClassLogger();
        private static IAuthorizationContextBuilder GetBuilder()
        {
            IAuthorizationContextBuilder builder;
            try
            {
                builder = ServiceLocator.Current.GetInstance<IAuthorizationContextBuilder>();
            }
            catch (Exception ex)
            {
                Log.Info("Faild to inject IAuthorizationContextBuilder", ex);
                builder = new AuthorizationContextBuilder();
            }
            return builder;
        }

        private static IAuthorizationProvider GetProvider()
        {
            try
            {
                IAuthorizationProvider provider = ServiceLocator.Current.GetInstance<IAuthorizationProvider>();
                return provider;
            }
            catch (Exception x)
            {
                Log.Error("Failed to inject the AuthorizationProvider", x);
                throw;
            }
        }

        private static IAuthorizationResponseBuilder GetResponseBuilder()
        {
            try
            {
                IAuthorizationResponseBuilder builder = ServiceLocator.Current.GetInstance<IAuthorizationResponseBuilder>();
                return builder;
            }
            catch (Exception x)
            {
                Log.Info("Failed to inject IAuthorizationResponseBuilder", x);
                return new AuthorizationResponseBuilder();
            }
        }
        public static IAuthorizationContext ToAuthorizationContext(this HttpRequest reqeust)
        {
            return ToAuthorizationContext(new HttpRequestWrapper(reqeust));
        }
        public static IAuthorizationContext ToAuthorizationContext(this HttpRequestBase request)
        {
            IAuthorizationContextBuilder builder = GetBuilder();

            return builder.FromHttpRequest(request);
        }

        public static IAuthorizationContext ToAuthorizationContext(this Uri uri)
        {
            IAuthorizationContextBuilder builder = GetBuilder();

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
       


    }
}
