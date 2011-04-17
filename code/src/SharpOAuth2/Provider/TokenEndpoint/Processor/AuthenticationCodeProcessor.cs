﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Globalization;
using SharpOAuth2.Provider;
namespace SharpOAuth2.Provider.TokenEndpoint.Processor
{
    public class AuthenticationCodeProcessor : ContextProcessor<ITokenContext>
    {
        public AuthenticationCodeProcessor(IServiceFactory serviceFactory) 
            :base(serviceFactory){}
        

        #region IContextProcessor<ITokenContext> Members

        public override bool IsSatisfiedBy(ITokenContext context)
        {
            return context.GrantType == Parameters.GrantTypeValues.AuthorizationCode;
        }

        public override void Process(ITokenContext context)
        {
            AuthorizationGrantBase grant = ServiceFactory.TokenService.FindAuthorizationGrant(context.AuthorizationCode);

            
            if (grant == null|| !grant.IsApproved)
                throw Errors.InvalidGrant(context);

            if (!ServiceFactory.TokenService.ValidateRedirectUri(context, grant))
                throw Errors.InvalidGrant(context);

            if (grant.ExpiresIn > 0 && (grant.IssuedOn + grant.ExpiresIn) < DateTime.Now.ToEpoch())
                throw Errors.InvalidGrant(context);

            if (!ServiceFactory.ClientService.AuthenticateClient(context))
                throw Errors.InvalidClient(context);

            ClientBase client = ServiceFactory.ClientService.FindClient(context.Client.ClientId);

            if (client.ClientId != grant.Client.ClientId)
                throw Errors.InvalidGrant(context);

            context.Token = ServiceFactory.TokenService.MakeAccessToken(grant);

            ServiceFactory.TokenService.ConsumeGrant(grant);

            


        }

        #endregion
    }
}
