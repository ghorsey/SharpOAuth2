using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint
{
    [TestFixture]
    public class TokenResponseBuilderTests
    {
        [Test]
        public void TestInvalidGrantContext()
        {
            TokenContext context = new TokenContext
            {
                Error = new ErrorResponse
                {
                    Error = Parameters.ErrorParameters.ErrorValues.InvalidGrant
                }
            };

            TokenResponseBuilder builder = new TokenResponseBuilder();

            TokenResponse response = builder.CreateResponse(context);

            Assert.AreEqual(400, response.HttpStatusCode);
            Assert.AreEqual("{\"error\":\"invalid_grant\"}", response.Body);
        }

        [Test]
        public void TestInvalidClientContext()
        {
            TokenContext context = new TokenContext
            {
                Error = new ErrorResponse
                {
                    Error = Parameters.ErrorParameters.ErrorValues.InvalidClient,
                    ErrorDescription = string.Empty
                }
            };

            TokenResponseBuilder builder = new TokenResponseBuilder();

            TokenResponse response = builder.CreateResponse(context);

            Assert.AreEqual(401, response.HttpStatusCode);
            Assert.AreEqual("{\"error\":\"invalid_client\"}", response.Body);
        }


        [Test]
        public void TestValidClientContext()
        {
            TokenContext context = new TokenContext
            {
                Token = new AccessTokenBase
                {
                    ExpiresIn = 3600,
                    Token = "token",
                    RefreshToken = "refresh",
                    TokenType = "bearer"
                }
            };

            TokenResponseBuilder builder = new TokenResponseBuilder();

            TokenResponse response = builder.CreateResponse(context);

            Assert.AreEqual(200, response.HttpStatusCode);
            Assert.AreEqual("{\"access_token\":\"token\",\"expires_in\":3600,\"refresh_token\":\"refresh\",\"token_type\":\"bearer\"}", response.Body);
        }

        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestNoTokenInContext()
        {
            TokenContext context = new TokenContext();

            TokenResponseBuilder builder = new TokenResponseBuilder();

            TokenResponse response = builder.CreateResponse(context);

        }
    }
}
