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
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.TokenEndpoint.Inspectors;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public class TokenProvider : ITokenProvider
    {

        private void InspectContext(ITokenContext context)
        {
            new SpecificationInspector().Inspect(context);
            new AuthorizationCodeInspector().Inspect(context);
            new RefreshAccessTokenInspector().Inspect(context);
            new ResourceOwnerPasswordCredentialInspector().Inspect(context);

            IEnumerable<IContextInspector<ITokenContext>> inspectors = ServiceLocator.Current.GetAllInstances<IContextInspector<ITokenContext>>();

            foreach (IContextInspector<ITokenContext> inspector in inspectors)
                inspector.Inspect(context);
        }
        #region ITokenProvider Members

        public void GrantAccessToken(ITokenContext context)
        {
            try
            {
                InspectContext(context);

                bool handled = false;

                var processors = ServiceLocator.Current.GetAllInstances<ContextProcessor<ITokenContext>>();

                foreach (ContextProcessor<ITokenContext> processor in processors)
                {
                    if (!processor.IsSatisfiedBy(context)) continue;

                    handled = true;
                    processor.Process(context);
                }

                if (!handled)
                    throw Errors.UnsupportedGrantType(context);
            }
            catch (OAuthErrorResponseException<ITokenContext> x)
            {
                context.Error = new ErrorResponse
                {
                    Error = x.Error,
                    ErrorDescription = x.Message,
                    ErrorUri = x.ErrorUri
                };
            }
        }

        #endregion
    }
}
