using System;
using Moq;
using NUnit.Framework;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.TokenEndpoint.Processor;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Processors
{
    [TestFixture]
    public class RefreshTokenProcessorTests
    {
        [Test]
        public void TestProcessorSatisfiedBy()
        {
            TokenContext context = new TokenContext { GrantType = Parameters.GrantTypeValues.AuthorizationCode };

            RefreshTokenProcessor processor = new RefreshTokenProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsFalse(processor.IsSatisfiedBy(context));
            context.GrantType = Parameters.GrantTypeValues.RefreshToken;
            Assert.IsTrue(processor.IsSatisfiedBy(context));
        }

        [Test]
        public void TestExchangingRefreshTokenForAccessToken()
        {
            ClientBase client = new ClientBase{ ClientId = "id", ClientSecret = "secret" };
            AccessTokenBase token = new AccessTokenBase{
                ExpiresIn = 120,
                RefreshToken = "refresh_token",
                Token = "new-token",
                TokenType = Parameters.AccessTokenTypeValues.Bearer
            };
            TokenContext context = new TokenContext
            {
                RefreshToken = "refresh_token",
                Client = client,
                ResourceOwnerUsername = "owner"
            };
            RefreshTokenBase refreshToken = new RefreshTokenBase
            {
                ClientId = "id",
                ResourceOwnerUsername = "owner",
                Scope = new string[] { "create", "delete" },
                Token = "refresh_token"
            };

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);
            mckClientService.Setup(x => x.FindClient("id")).Returns(client);

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.FindRefreshToken("refresh_token")).Returns(refreshToken);
            mckTokenService.Setup(x => x.IssueAccessToken(refreshToken)).Returns(token);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);

            RefreshTokenProcessor processor = new RefreshTokenProcessor(mckFactory.Object);

            processor.Process(context);

            Assert.AreEqual(token, context.Token);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckTokenService.VerifyAll();
        }

        [Test]
        public void TestExchangingRefreshTokenForWrongClient()
        {
            ClientBase client = new ClientBase { ClientId = "id", ClientSecret = "secret" };
            
            TokenContext context = new TokenContext
            {
                RefreshToken = "refresh_token",
                Client = new ClientBase { ClientId = "id" },
                ResourceOwnerUsername = "owner"
            };
            RefreshTokenBase refreshToken = new RefreshTokenBase
            {
                ClientId = "123",
                ResourceOwnerUsername = "owner",
                Scope = new string[] { "create", "delete" },
                Token = "refresh_token"
            };

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);
            mckClientService.Setup(x => x.FindClient("id")).Returns(client);

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.FindRefreshToken("refresh_token")).Returns(refreshToken);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);

            RefreshTokenProcessor processor = new RefreshTokenProcessor(mckFactory.Object);

            try
            {
                processor.Process(context);
                Assert.Fail("no exception trhown");
            }
            catch (OAuthErrorResponseException<IOAuthContext> x)
            {
                Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.InvalidClient, x.Error);
            }
            catch (Exception x)
            {
                Assert.Fail("unexpected exception: " + x.Message);
            }

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckTokenService.VerifyAll();
        }

        [Test]
        public void TestInvalidRefreshTokenForClient()
        {
            ClientBase client = new ClientBase { ClientId = "id", ClientSecret = "secret" };

            TokenContext context = new TokenContext
            {
                RefreshToken = "refresh_token",
                Client = client
            };

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);
            mckClientService.Setup(x => x.FindClient("id")).Returns(client);

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.FindRefreshToken("refresh_token")).Returns((RefreshTokenBase)null);


            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);

            RefreshTokenProcessor processor = new RefreshTokenProcessor(mckFactory.Object);
            
            AssertExceptionResponse(context, processor, Parameters.ErrorParameters.ErrorValues.InvalidRequest, 400);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckTokenService.VerifyAll();
        }


        [Test]
        public void TestInvalidClientExchangingRefreshToken()
        {
            ClientBase client = new ClientBase { ClientId = "id", ClientSecret = "secret" };

            TokenContext context = new TokenContext
            {
                RefreshToken = "refresh_token",
                Client = client
            };

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(false);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);

            RefreshTokenProcessor processor = new RefreshTokenProcessor(mckFactory.Object);

            AssertExceptionResponse(context, processor, Parameters.ErrorParameters.ErrorValues.UnauthorizedClient, 401);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
        }

        private static void AssertExceptionResponse(TokenContext context, RefreshTokenProcessor processor, string parameter, int httpStatusCode)
        {
            try
            {
                processor.Process(context);
                Assert.Fail("no exception thrown");
            }
            catch (OAuthErrorResponseException<ITokenContext> x)
            {
                Assert.AreEqual(parameter, x.Error);
                Assert.AreEqual(httpStatusCode, x.HttpStatusCode);
            }
            catch (Exception x)
            {
                Assert.Fail("Unexpected exception: " + x.Message);
            }
        }

    }
}
