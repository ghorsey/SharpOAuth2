using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.Services;

namespace SharpOAuthProvider.Domain.Service
{
    public class ServiceFactory : IAuthorizationServiceFactory
    {

        public ServiceFactory(IClientService clientService, ITokenService tokenService)
        {
            ClientService = clientService;
            TokenService = tokenService;
        }

        #region IAuthorizationServiceFactory Members

        public IClientService ClientService{  get; private set; }
        public ITokenService TokenService{ get; private set; }

        #endregion
    }
}
