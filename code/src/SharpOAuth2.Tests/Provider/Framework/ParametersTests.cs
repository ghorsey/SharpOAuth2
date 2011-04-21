using NUnit.Framework;
using SharpOAuth2.Framework;
namespace SharpOAuth2.Tests.Provider.Framework
{
    [TestFixture]
    public class ParametersTests
    {
        [Test]
        public void TestParameters()
        {
            Assert.AreEqual("access_token", Parameters.AccessToken);
            Assert.AreEqual("token_type", Parameters.AccessTokenType);
            Assert.AreEqual("expires_in", Parameters.AccessTokenExpiresIn);
            Assert.AreEqual("code", Parameters.AuthroizationCode);
            Assert.AreEqual("client_id", Parameters.ClientId);
            Assert.AreEqual("client_secret", Parameters.ClientSecret);
            Assert.AreEqual("grant_type", Parameters.GrantType);
            Assert.AreEqual("redirect_uri", Parameters.RedirectUri);
            Assert.AreEqual("refresh_token", Parameters.RefreshToken);
            Assert.AreEqual("password", Parameters.ResourceOwnerPassword);
            Assert.AreEqual("username", Parameters.ResourceOwnerUsername);
            Assert.AreEqual("response_type", Parameters.ResponseType);
            Assert.AreEqual("scope", Parameters.Scope);
            Assert.AreEqual("state", Parameters.State);
        }

        [Test]
        public void TestErrorParameters()
        {
            Assert.AreEqual("error", Parameters.ErrorParameters.Error);
            Assert.AreEqual("error_description", Parameters.ErrorParameters.ErrorDescription);
            Assert.AreEqual("error_uri", Parameters.ErrorParameters.ErrorUri);

            Assert.AreEqual("access_denied", Parameters.ErrorParameters.ErrorValues.AccessDenied);
            Assert.AreEqual("invalid_request", Parameters.ErrorParameters.ErrorValues.InvalidRequest);
            Assert.AreEqual("invalid_scope", Parameters.ErrorParameters.ErrorValues.InvalidScope);
            Assert.AreEqual("unauthorized_client", Parameters.ErrorParameters.ErrorValues.UnauthorizedClient);
            Assert.AreEqual("unsupported_response_type", Parameters.ErrorParameters.ErrorValues.UnsupportedResponseType);
        }

        [Test]
        public void TestAccessTokenTypeValues()
        {
            Assert.AreEqual("bearer", Parameters.AccessTokenTypeValues.Bearer);
            Assert.AreEqual("mac", Parameters.AccessTokenTypeValues.Mac);

            Assert.AreEqual("algorithm", Parameters.AccessTokenTypeValues.MacParameters.Algorithm);
            Assert.AreEqual("secret", Parameters.AccessTokenTypeValues.MacParameters.Secret);

            Assert.AreEqual("hmac-sha-1", Parameters.AccessTokenTypeValues.MacParameters.AlgorithmValues.HmacSha1);
            Assert.AreEqual("hmac-sha-256", Parameters.AccessTokenTypeValues.MacParameters.AlgorithmValues.HmacSha256);
        }

        [Test]
        public void TestGrantTypeValues()
        {
            Assert.AreEqual("authorization_code", Parameters.GrantTypeValues.AuthorizationCode);
            Assert.AreEqual("client_credentials", Parameters.GrantTypeValues.ClientCredentials);
            Assert.AreEqual("password", Parameters.GrantTypeValues.Password);
            Assert.AreEqual("refresh_token", Parameters.GrantTypeValues.RefreshToken);
        }
        [Test]
        public void TestResponseTypeValues()
        {
            Assert.AreEqual("code", Parameters.ResponseTypeValues.AuthorizationCode);
            Assert.AreEqual("token", Parameters.ResponseTypeValues.AccessToken);
        }
    }
}
