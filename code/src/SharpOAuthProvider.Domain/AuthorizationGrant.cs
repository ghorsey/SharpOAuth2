using SharpOAuth2.Provider.Domain;

namespace SharpOAuthProvider.Domain
{
    public class AuthorizationGrant : AuthorizationGrantBase
    {
        public bool IsUsed { get; set; }
        public string ResourceOwnerId { get; set; }
    }
}
