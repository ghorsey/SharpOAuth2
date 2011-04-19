using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Provider.TokenEndpoint.Processor
{
    public class ResourceOwnerPasswordCredentialProcessor : ContextProcessor<ITokenContext>
    {
        public ResourceOwnerPasswordCredentialProcessor(IServiceFactory serviceFactory) : base(serviceFactory) { }

        public override bool IsSatisfiedBy(ITokenContext context)
        {
            return context.GrantType.ToUpperInvariant() == Parameters.GrantTypeValues.Password.ToUpperInvariant();
        }

        public override void Process(ITokenContext context)
        {
            if (!ServiceFactory.ClientService.AuthenticateClient(context))
                throw Errors.InvalidClient(context);

            if( !ServiceFactory.ResourceOwnerService.CredentialsAreValid(context.ResourceOwnerUsername, context.ResourceOwnerPassword))
                throw Errors.InvalidRequestException(context, Parameters.ResourceOwnerUsername);

            context.Token = ServiceFactory.TokenService.MakeAccessToken(context.ResourceOwnerUsername);
        }
    }
}
