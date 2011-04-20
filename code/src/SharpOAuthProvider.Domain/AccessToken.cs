using SharpOAuth2.Provider.Domain;

namespace SharpOAuthProvider.Domain
{
    public class AccessToken : AccessTokenBase
    {
        public AuthorizationGrant Grant { get; set; }
        public string ResourceOwnerUsername { get; set; }
        public Client Client { get; set; }
        public AccessToken()
        {
            TokenType = "bearer";
        }
    }
}
