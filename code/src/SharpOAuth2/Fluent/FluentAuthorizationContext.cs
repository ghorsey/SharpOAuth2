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
    }
}
