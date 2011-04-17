using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Globalization;

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
            }

            if (!handled)
                throw new OAuthFatalException(ResourceEndpointResources.UnsupportedTokenType);

            if (context.Token == null)
                throw Errors.InvalidToken(context);

            if (context.Token.ExpiresIn > 0 && (context.Token.IssuedOn + context.Token.ExpiresIn) < DateTime.Now.ToEpoch())
                throw Errors.InvalidToken(context);

        }

        #endregion
    }
}
