using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.Authorization.Services
{
    public interface IClientService
    {
        bool AuthenticateClient(IClient client);
        bool ValidateRedirectUri(IAuthorizationContext context);
    }
}
