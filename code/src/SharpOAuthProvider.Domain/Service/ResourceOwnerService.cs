using SharpOAuth2.Provider.Services;

namespace SharpOAuthProvider.Domain.Service
{
    public class ResourceOwnerService : IResourceOwnerService
    {
        #region IResourceOwnerService Members

        public bool CredentialsAreValid(string resourceOwnerId, string resourceOwnerPassword)
        {
            return (resourceOwnerId.ToUpperInvariant() == "GEOFF" && resourceOwnerPassword == "password");
        }

        #endregion
    }
}
