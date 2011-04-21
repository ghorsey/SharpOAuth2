using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.Framework;
using Moq;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.TokenEndpoint.Processor;
using SharpOAuth2.Provider.Domain;

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
                Client = client
            };

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);
            mckClientService.Setup(x => x.FindClient("id")).Returns(client);

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.ValidateRefreshTokenForClient("refresh_token", client)).Returns(true);
            mckTokenService.Setup(x => x.IssueAccessToken("refresh_token", client)).Returns(token);

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
            mckTokenService.Setup(x => x.ValidateRefreshTokenForClient("refresh_token", client)).Returns(false);


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
