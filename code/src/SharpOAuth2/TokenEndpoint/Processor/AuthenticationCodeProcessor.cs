#region License
/* The MIT License
 * 
 * Copyright (c) 2011 Geoff Horsey
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.Utility;

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
