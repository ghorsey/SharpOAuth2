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

using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
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
            AuthorizationGrantBase grant = ServiceFactory.TokenService.IssueAuthorizationGrant(context);
            grant.Scope = context.Scope;
            grant.RedirectUri = context.RedirectUri;
            grant.IsApproved = context.IsApproved;
            grant.ResourceOwnerUsername = context.ResourceOwnerUsername;
            ServiceFactory.TokenService.ApproveAuthorizationGrant(grant, context.IsApproved);
            context.Token = grant;
        }
    }
}
