using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Globalization;
using System.Collections.Specialized;

namespace SharpOAuth2.Provider.ResourceEndpoint.Processors
{
    public class BearerProcessor : ContextProcessor<IResourceContext>
    {
        public BearerProcessor(IServiceFactory serviceFactory) : base(serviceFactory) { }

        private string ParseBearerToken(IResourceContext context)
        {
            if (!string.IsNullOrWhiteSpace((context.QueryString ?? new NameValueCollection())[Parameters.BearerTokenValues.BearerTokenRequest]))
                return context.QueryString[Parameters.BearerTokenValues.BearerTokenRequest];
            if (!string.IsNullOrWhiteSpace((context.Form ?? new NameValueCollection())[Parameters.BearerTokenValues.BearerTokenRequest]))
                return context.Form[Parameters.BearerTokenValues.BearerTokenRequest];

            if(string.IsNullOrWhiteSpace((context.Headers ?? new NameValueCollection())[Parameters.HttpAuthorizationHeader]))
                return string.Empty;

            string value = context.Headers[Parameters.HttpAuthorizationHeader]; 
            string type = value.Substring(0, value.IndexOf(" ")).ToUpperInvariant();
            if( type != "BEARER")
                return string.Empty;

            return value.Substring(value.IndexOf(" ") + 1);

        }

        public override bool IsSatisfiedBy(IResourceContext context)
        {
            return !string.IsNullOrWhiteSpace(ParseBearerToken(context));
        }

        public override void Process(IResourceContext context)
        {
            string rawToken = ParseBearerToken(context);

            AccessTokenBase token = ServiceFactory.TokenService.FindToken(rawToken);

            


            context.Token = token;
        }

    }
}
