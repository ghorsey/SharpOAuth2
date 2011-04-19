using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Provider.TokenEndpoint.Processor
{
    public class ClientCredentialsProcessor : ContextProcessor<ITokenContext>
    {
        public ClientCredentialsProcessor(IServiceFactory serviceFactory) : base(serviceFactory) { }


        public override bool IsSatisfiedBy(ITokenContext context)
        {
            return context.GrantType == Parameters.GrantTypeValues.ClientCredentials;
        }

        public override void Process(ITokenContext context)
        {

            if (!ServiceFactory.ClientService.AuthenticateClient(context))
                throw Errors.UnauthorizedClient(context, context.Client);


            ClientBase client = ServiceFactory.ClientService.FindClient(context.Client.ClientId);

            context.Token = ServiceFactory.TokenService.MakeAccessToken(client);
        }
    }
}
