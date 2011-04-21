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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Globalization;
using SharpOAuth2.Provider.Utility;

namespace SharpOAuth2.Provider.ResourceEndpoint
{
    public class ResourceProvider  : IResourceProvider
    {
        #region IResourceProvider Members

        public void AccessProtectedResource(IResourceContext context)
        {

            IEnumerable<ContextProcessor<IResourceContext>> processors = ServiceLocator.Current.GetAllInstances<ContextProcessor<IResourceContext>>();

            bool handled = false;
            foreach( ContextProcessor<IResourceContext> processor in processors)
            {
                if( !processor.IsSatisfiedBy(context)) continue;
                processor.Process(context);
                handled = true;
                break;
            }

            if (!handled)
                throw new OAuthFatalException(ResourceEndpointResources.UnsupportedTokenType);

            if (context.Token == null)
                throw Errors.InvalidToken(context);

            if (context.Token.ExpiresIn > 0 && (context.Token.IssuedOn + context.Token.ExpiresIn) < DateTime.Now.ToEpoch())
                throw Errors.InvalidToken(context);

        }


        public void ValidateScope(IResourceContext context, string[] scope)
        {
            if (context.Token == null)
                throw Errors.InvalidToken(context);

            foreach (string scopeItem in scope)
            {
                if (context.Token.Scope.Where(x => x.ToUpperInvariant() == scopeItem.ToUpperInvariant()).Count() != 1)
                    throw Errors.InsufficientScope(context, string.Join(" ", scope));
            }
        }

        #endregion
    }
}
