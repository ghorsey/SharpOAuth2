using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;

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
            throw new NotImplementedException();
        }

        #endregion
    }
}
