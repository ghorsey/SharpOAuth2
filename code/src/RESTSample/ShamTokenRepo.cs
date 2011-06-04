using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharpOAuthProvider.Domain;
using SharpOAuthProvider.Domain.Repository;

namespace RESTSample
{
    /*
     * This is a really lame file, I just don't have time working to get a shared repository going... 
     * but the proof of concept is here.
     */
    public class ShamTokenRepo : ITokenRepository
    {
        #region ITokenRepository Members

        public void AddAccessToken(SharpOAuthProvider.Domain.AccessToken token)
        {
            throw new NotImplementedException();
        }

        public void AddAuthorizationGrant(SharpOAuthProvider.Domain.AuthorizationGrant grant)
        {
            throw new NotImplementedException();
        }

        public SharpOAuthProvider.Domain.AuthorizationGrant FindAuthorizationGrant(string authorizationCode)
        {
            throw new NotImplementedException();
        }

        public SharpOAuthProvider.Domain.AuthorizationGrant FindAuthorizationGrant(string clientId, string resourceOwnerId)
        {
            throw new NotImplementedException();
        }

        public SharpOAuth2.Provider.Domain.AccessTokenBase FindToken(string token)
        {
            AccessToken sham = new AccessToken
            {
                Token = token,
                Scope = new string[] { "view", "edit" },
            };
            return sham;
        }

        public SharpOAuth2.Provider.Domain.RefreshTokenBase FindRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}