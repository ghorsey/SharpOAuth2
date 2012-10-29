using SharpOAuth2.Provider.Domain;

namespace SharpOAuthProvider.Domain
{
    public class AuthorizationGrant : AuthorizationGrantBase
    {
        public Client Client { get; set; }
        public bool IsApproved { get; set; }
        public int ExpiresIn { get; set; }
        public long IssuedOn { get; set; }
        public bool IsUsed { get; set; }
        public string ResourceOwnerId { get; set; }
		public string Scope { get; set; }
    }
}
