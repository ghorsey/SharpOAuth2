using SharpOAuth2.Provider.Domain;

namespace SharpOAuthProvider.Domain.Repository
{
    public interface ITokenRepository
    {
        void AddAccessToken(AccessToken token);
        void AddAuthorizationGrant(AuthorizationGrant grant);

        AuthorizationGrant FindAuthorizationGrant(string authorizationCode);
        AuthorizationGrant FindAuthorizationGrant(string clientId, string resourceOwnerId);
        AccessTokenBase FindToken(string token);
        RefreshTokenBase FindRefreshToken(string refreshToken);
    }
}
