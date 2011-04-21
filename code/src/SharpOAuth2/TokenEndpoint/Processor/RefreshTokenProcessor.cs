using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.Domain;

namespace SharpOAuth2.Provider.TokenEndpoint.Processor
{
    public class RefreshTokenProcessor : ContextProcessor<ITokenContext>
    {
        public RefreshTokenProcessor(IServiceFactory serviceFactory) : base(serviceFactory) { }
        public override bool IsSatisfiedBy(ITokenContext context)
        {
            return context.GrantType == Parameters.GrantTypeValues.RefreshToken;
        }

        public override void Process(ITokenContext context)
        {
            if (!ServiceFactory.ClientService.AuthenticateClient(context))
                throw Errors.UnauthorizedClient(context, context.Client);

            ClientBase client = ServiceFactory.ClientService.FindClient(context.Client.ClientId);

            if (!ServiceFactory.TokenService.ValidateRefreshTokenForClient(context.RefreshToken, client))
                throw Errors.InvalidRequestException(context, Parameters.RefreshToken);

            context.Token = ServiceFactory.TokenService.IssueAccessToken(context.RefreshToken, client);
        }
    }
}
