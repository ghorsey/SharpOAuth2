using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.TokenEndpoint;

namespace SharpOAuthProvider.Domain.Service
{
    public class ResourceOwnerService : IResourceOwnerService
    {
        #region IResourceOwnerService Members

        public bool CredentialsAreValid(ITokenContext context)
        {
            return (context.ResourceOwnerUsername.ToUpperInvariant() == "GEOFF" && context.ResourceOwnerPassword == "password");
        }

        #endregion
    }
}
