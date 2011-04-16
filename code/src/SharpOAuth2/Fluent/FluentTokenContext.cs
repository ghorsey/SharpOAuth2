using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider;
using Microsoft.Practices.ServiceLocation;
using Common.Logging;
using System.Web;

namespace SharpOAuth2.Fluent
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

        public static ITokenContext GrantAuthorizationToken(this ITokenContext context)
        {
            ITokenProvider provider = GetProvider();

            provider.GrantAuthorizationToken(context);
            return context;
        }

        public static TokenResponse CreateTokenResponse(this ITokenContext context)
        {
            ITokenResponseBuilder builder = GetResponseBuilder();
            return builder.CreateResponse(context);
        }
    }
}
