using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.TokenEndpoint.Inspectors;
using Microsoft.Practices.ServiceLocation;

namespace SharpOAuth2.Provider.TokenEndpoint
{
    public class TokenProvider : ITokenProvider
    {
        readonly IServiceFactory ServiceFactory;

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

        public void GrantAuthorizationToken(ITokenContext context)
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
