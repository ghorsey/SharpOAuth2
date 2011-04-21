using System.Collections.Generic;
using NUnit.Framework;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Domain;

namespace SharpOAuth2.Tests.Provider.Domain
{
    [TestFixture]
    public class AccessTokenBaseTests
    {
        [Test]
        public void ConvertAccessTokenToNameValuePairs()
        {
            AccessTokenBase token = new AccessTokenBase
            {
                ExpiresIn = 120,
                RefreshToken = "refresh_token",
                Scope = new string[] { "create", "delete" },
                Token = "token",
                TokenType = "bearer"
            };

            token.Parameters["token_secret"] = "secret_val";

            IDictionary<string, object> results = token.ToResponseValues();

            Assert.AreEqual(120, results[Parameters.AccessTokenExpiresIn], Parameters.AccessTokenExpiresIn);
            Assert.AreEqual("refresh_token", results[Parameters.RefreshToken], Parameters.RefreshToken);
            Assert.AreEqual("create delete", results[Parameters.Scope], Parameters.Scope);
            Assert.AreEqual("token", results[Parameters.AccessToken], Parameters.AccessToken);
            Assert.AreEqual("bearer", results[Parameters.AccessTokenType], Parameters.AccessTokenType);
            Assert.AreEqual("secret_val", results["token_secret"]);
        }
    }
}
