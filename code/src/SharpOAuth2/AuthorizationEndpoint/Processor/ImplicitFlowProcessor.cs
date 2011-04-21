using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.Domain;

namespace SharpOAuth2.Provider.AuthorizationEndpoint.Processor
{
    public class ImplicitFlowProcessor : ContextProcessor<IAuthorizationContext>
    {
        public ImplicitFlowProcessor(IServiceFactory serviceFactory) : base(serviceFactory) { }

        public override bool IsSatisfiedBy(IAuthorizationContext context)
        {
            return context.ResponseType == Parameters.ResponseTypeValues.AccessToken;
        }

        public override void Process(IAuthorizationContext context)
        {
            ClientBase client = ServiceFactory.ClientService.FindClient(context.Client.ClientId);

            AuthorizationGrantBase grant = ServiceFactory.TokenService.IssueAuthorizationGrant(context);

            context.Token = ServiceFactory.TokenService.IssueAccessToken(grant);

        }
    }
}
