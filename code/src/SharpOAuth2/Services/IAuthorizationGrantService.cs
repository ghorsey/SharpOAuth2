using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.Domain;

namespace SharpOAuth2.Provider.Services
{
    public interface IAuthorizationGrantService
    {
        AuthorizationGrantBase FindAuthorizationGrant(string authorizationCode);
        AuthorizationGrantBase IssueAuthorizationGrant(IAuthorizationContext context);
        void ConsumeGrant(AuthorizationGrantBase grant);
        bool ValidateGrant(TokenEndpoint.ITokenContext context, AuthorizationGrantBase grant);
    }
}
