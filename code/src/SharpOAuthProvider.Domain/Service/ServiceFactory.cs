using SharpOAuth2.Provider.Services;

namespace SharpOAuthProvider.Domain.Service
{
    public class ServiceFactory : IServiceFactory
    {

        public ServiceFactory(IClientService clientService, ITokenService tokenService, IResourceOwnerService resourceOwnerService, IAuthorizationGrantService authorizationGrantService)
        {
            ClientService = clientService;
            TokenService = tokenService;
            ResourceOwnerService = resourceOwnerService;
            AuthorizationGrantService = authorizationGrantService;
        }

        #region IAuthorizationServiceFactory Members
        public IAuthorizationGrantService AuthorizationGrantService{ get; private set; }
        public IClientService ClientService{  get; private set; }
        public ITokenService TokenService{ get; private set; }
        public IResourceOwnerService ResourceOwnerService { get; private set; }
        #endregion
    }
}
