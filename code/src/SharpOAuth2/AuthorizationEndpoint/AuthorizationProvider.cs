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

using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.AuthorizationEndpoint.Inspectors;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Globalization;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Provider.AuthorizationEndpoint
{
    public class AuthorizationProvider : IAuthorizationProvider
    {
        readonly IServiceFactory ServiceFactory;

        public AuthorizationProvider(IServiceFactory serviceFactory)
        {
            ServiceFactory = serviceFactory;
        }
        private void InspectRequest(IAuthorizationContext context)
        {
            AssertNoAuthorizationToken(context);
            AssertIsClient(context);
            AssertRedirectUriIsValid(context);
            AssertResourceOwnerIdIsNotBlank(context);

            new SpecificationInspector().Inspect(context); // always run the known inspectors
            IEnumerable<IContextInspector<IAuthorizationContext>> inspectors = ServiceLocator.Current.GetAllInstances<IContextInspector<IAuthorizationContext>>();

            foreach (IContextInspector<IAuthorizationContext> inspector in inspectors)
                inspector.Inspect(context);
        }
        private void AssertResourceOwnerIdIsNotBlank(IAuthorizationContext context)
        {
            if (string.IsNullOrWhiteSpace(context.ResourceOwnerUsername))
                throw new OAuthFatalException(AuthorizationEndpointResources.ResourceOwnerNotIncluded);
        }
        private void AssertNoAuthorizationToken(IAuthorizationContext context)
        {
            if (context.Token != null)
                throw new OAuthFatalException(AuthorizationEndpointResources.AuthorizationContextContainsToken);
        }

        private void AssertIsClient(IAuthorizationContext context)
        {
            if (!ServiceFactory.ClientService.IsClient(context))
                throw Errors.UnauthorizedClient(context, context.Client);
        }

        private void AssertRedirectUriIsValid(IAuthorizationContext context)
        {
            if (!ServiceFactory.ClientService.ValidateRedirectUri(context))
                throw new OAuthFatalException(string.Format(CultureInfo.CurrentUICulture,
                    AuthorizationEndpointResources.InvalidRedirectUri, context.RedirectUri.ToString()));
        }
        #region IAuthorizationProvider Members

        public void CreateAuthorizationGrant(IAuthorizationContext context)
        {
            try
            {
                InspectRequest(context);
               

                IEnumerable<ContextProcessor<IAuthorizationContext>> processors = ServiceLocator.Current.GetAllInstances<ContextProcessor<IAuthorizationContext>>();

                bool handled = false;
                foreach (ContextProcessor<IAuthorizationContext> processor in processors)
                {
                    if (!processor.IsSatisfiedBy(context)) continue;
                    processor.Process(context);
                    handled = true;
                    break;
                }

                if (!handled)
                    throw Errors.UnsupportedResponseType(context, context.ResponseType);

                if (!context.IsApproved)
                    throw Errors.AccessDenied(context);
            }
            catch (OAuthErrorResponseException<IAuthorizationContext> ex)
            {
                context.Error = new ErrorResponse
                {
                    Error = ex.Error,
                    ErrorDescription = ex.Message,
                    ErrorUri = ex.ErrorUri
                };
            }
        }

        #endregion

        #region IAuthorizationProvider Members


        public bool IsAccessApproved(IAuthorizationContext context)
        {
            InspectRequest(context);

            return ServiceFactory.ClientService.IsAccessGranted(context.Client, context.Scope, context.ResourceOwnerUsername);
        }

        #endregion
    }
}
