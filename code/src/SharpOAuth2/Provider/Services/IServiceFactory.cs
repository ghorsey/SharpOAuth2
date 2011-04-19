using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpOAuth2.Provider.Services
{
    public interface IServiceFactory
    {
        IClientService ClientService { get; }
        ITokenService TokenService { get; }
        IResourceOwnerService ResourceOwnerService { get; }
    }
}
