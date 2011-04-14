using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Provider.AuthorizationEndpoint.Processor
{
    public class AuthorizationCodeProcessor : ContextProcessor<IAuthorizationContext>
    {
        public AuthorizationCodeProcessor(IServiceFactory serviceFactory) : base(serviceFactory) { }

        public override bool IsSatisfiedBy(IAuthorizationContext context)
        {
            return context.ResponseType == Parameters.ResponseTypeValues.AuthorizationCode;
        }

        public override void Process(IAuthorizationContext context)
        {
            AuthorizationGrantBase grant = ServiceFactory.TokenService.MakeAuthorizationGrant(context);
            ServiceFactory.TokenService.ApproveAuthorizationGrant(grant, context.IsApproved);
            context.Authorization = grant;
        }
    }
}
