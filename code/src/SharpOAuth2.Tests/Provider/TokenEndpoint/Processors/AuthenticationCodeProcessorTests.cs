using System;
using Moq;
using NUnit.Framework;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.TokenEndpoint.Processor;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Processors
{
    [TestFixture]
    public class AuthenticationCodeProcessorTests : ProcessorTestBase
    {
        [Test]
        public void TestSatisfiedByMethod()
        {
            TokenContext context = new TokenContext();
            context.GrantType = Parameters.GrantTypeValues.AuthorizationCode;

            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsTrue(processor.IsSatisfiedBy(context));
        }

        [Test]
        public void TestProcessingValidContext()
        {
            TokenContext context = new TokenContext()
            {
                Client = new ClientBase { ClientId = "321", ClientSecret = "secret" },
                AuthorizationCode = "123",
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                RedirectUri = new Uri("http://www.mysites.com/callback"),
                Scope = new string[] { "create", "delete" }
            };

            AuthorizationGrantBase authorizationGrant = new AuthorizationGrantBase { Code = "123"};

            Mock<IAuthorizationGrantService> mckAuthService = new Mock<IAuthorizationGrantService>();
            mckAuthService.Setup(x => x.FindAuthorizationGrant("123")).Returns(authorizationGrant);
            mckAuthService.Setup(x => x.ConsumeGrant(It.IsAny<AuthorizationGrantBase>()));
            mckAuthService.Setup(x => x.ValidateGrant(context, authorizationGrant)).Returns(true);

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.IssueAccessToken(authorizationGrant)).Returns(new AccessTokenBase { TokenType = "bearer", RefreshToken = "refresh_token", Token = "token", ExpiresIn = 120 });
            
            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);
            mckFactory.SetupGet(x => x.AuthorizationGrantService).Returns(mckAuthService.Object);
            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(mckFactory.Object);

            processor.Process(context);

            Assert.IsNotNull(context.Token);
            Assert.AreEqual(120, context.Token.ExpiresIn);
            Assert.AreEqual("refresh_token", context.Token.RefreshToken);
            Assert.AreEqual("token", context.Token.Token);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckTokenService.VerifyAll();
        }

        [Test]
        public void TestProcessingNullInvalidAuthorizationGrantContext()
        {
            TokenContext context = new TokenContext()
            {
                Client = new ClientBase { ClientId = "321", ClientSecret = "secret" },
                AuthorizationCode = "123",
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                RedirectUri = new Uri("http://www.mysites.com/callback"),
                Scope = new string[] { "create", "delete" }
            };

            Mock<IAuthorizationGrantService> mckAuthService = new Mock<IAuthorizationGrantService>();
            
            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);
            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.AuthorizationGrantService).Returns(mckAuthService.Object);
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);

            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(mckFactory.Object);

            CommonProcessorErrorAssert(processor, context, Parameters.ErrorParameters.ErrorValues.InvalidGrant);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckAuthService.VerifyAll();
        }

        [Test]
        public void TestProcessingInvalidClient()
        {
            TokenContext context = new TokenContext()
            {
                Client = new ClientBase { ClientId = "321", ClientSecret = "secret" },
                AuthorizationCode = "123",
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                RedirectUri = new Uri("http://www.mysites.com/callback"),
                Scope = new string[] { "create", "delete" }
            };

            Mock<IAuthorizationGrantService> mckAuthService = new Mock<IAuthorizationGrantService>();
            mckAuthService.Setup(x => x.FindAuthorizationGrant("123")).Returns(new AuthorizationGrantBase { Code = "123" });

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(false);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.AuthorizationGrantService).Returns(mckAuthService.Object);

            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(mckFactory.Object);
            CommonProcessorErrorAssert(processor, context, Parameters.ErrorParameters.ErrorValues.InvalidClient);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckAuthService.VerifyAll();
        }

        [Test]
        public void TestProcessingMismatchedRedirectUri()
        {
            TokenContext context = new TokenContext()
            {
                AuthorizationCode = "123",
                RedirectUri = new Uri("http://www.mysites.com/callback"),
                Client = new ClientBase { ClientId = "client", ClientSecret = "secret" }
            };

            Mock<IAuthorizationGrantService> mckAuthService = new Mock<IAuthorizationGrantService>();
            mckAuthService.Setup(x => x.FindAuthorizationGrant("123")).Returns(new AuthorizationGrantBase());

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.AuthorizationGrantService).Returns(mckAuthService.Object);
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);

            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(mckFactory.Object);
            CommonProcessorErrorAssert(processor, context, Parameters.ErrorParameters.ErrorValues.InvalidGrant);

            mckFactory.VerifyAll();
            mckAuthService.VerifyAll();
        }


        [Test]
        public void TestProcessingMismatchedClientTest()
        {
            TokenContext context = new TokenContext()
            {
                Client = new ClientBase { ClientId = "321", ClientSecret = "secret" },
                AuthorizationCode = "123",
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                RedirectUri = new Uri("http://www.mysites.com/callback"),
                Scope = new string[] { "create", "delete" }
            };

            Mock<IAuthorizationGrantService> mckAuthService = new Mock<IAuthorizationGrantService>();
            mckAuthService.Setup(x => x.FindAuthorizationGrant("123")).Returns(new AuthorizationGrantBase { Code = "123" });
            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.AuthorizationGrantService).Returns(mckAuthService.Object);


            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(mckFactory.Object);
            CommonProcessorErrorAssert(processor, context, Parameters.ErrorParameters.ErrorValues.InvalidGrant);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckAuthService.VerifyAll();
        }

    }
}
