using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.Services;
using Moq;
using SharpOAuth2.Provider.TokenEndpoint.Processor;
using SharpOAuth2.Provider;

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
                Client = new ClientBase{ ClientId="321", ClientSecret="secret"},
                AuthorizationCode = "123",
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                RedirectUri = new Uri("http://www.mysites.com/callback"),
                Scope = new string[] { "create", "delete" }
            };

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.FindAuthorizationGrant("123")).Returns(new AuthorizationGrantBase { Client = new ClientBase { ClientId = "321", ClientSecret = "secret" } });
            mckTokenService.Setup(x => x.AuthorizationGrantIsValid(It.IsAny<AuthorizationGrantBase>())).Returns(true);
            mckTokenService.Setup(x => x.SetAccessToken(context)).Callback<ITokenContext>(x=>{
                x.Token = new AccessTokenBase
                {
                    Expires = 3600,
                    RefreshToken = "refresh-token",
                    Token = "token-value",
                    TokenType = Parameters.AccessTokenTypeValues.Bearer
                };
            });
            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.FindClient("321")).Returns(new ClientBase { ClientSecret = "secret", ClientId = "321" });
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);

            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(mckFactory.Object);

            processor.Process(context);

            Assert.IsNotNull(context.Token);
            Assert.AreEqual(3600, context.Token.Expires);
            Assert.AreEqual("refresh-token", context.Token.RefreshToken);
            Assert.AreEqual("token-value", context.Token.Token);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckTokenService.VerifyAll();
        }

        [Test]
        public void TestProcessingInValidAuthorizationGrantContext()
        {
            TokenContext context = new TokenContext()
            {
                Client = new ClientBase { ClientId = "321", ClientSecret = "secret" },
                AuthorizationCode = "123",
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                RedirectUri = new Uri("http://www.mysites.com/callback"),
                Scope = new string[] { "create", "delete" }
            };

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.AuthorizationGrantIsValid(It.IsAny<AuthorizationGrantBase>())).Returns(false);
            Mock<IClientService> mckClientService = new Mock<IClientService>();

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);

            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(mckFactory.Object);

            CommonProcessorErrorAssert(processor, context, Parameters.ErrorParameters.ErrorValues.InvalidGrant);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckTokenService.VerifyAll();
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

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.FindAuthorizationGrant("123")).Returns(new AuthorizationGrantBase { Client = new ClientBase { ClientId = "321", ClientSecret = "secret" } });
            mckTokenService.Setup(x => x.AuthorizationGrantIsValid(It.IsAny<AuthorizationGrantBase>())).Returns(true);
            
            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(false);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);

            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(mckFactory.Object);
            CommonProcessorErrorAssert(processor, context, Parameters.ErrorParameters.ErrorValues.InvalidClient);

            
            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckTokenService.VerifyAll();
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

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.FindAuthorizationGrant("123")).Returns(new AuthorizationGrantBase { Client = new ClientBase { ClientId = "555", ClientSecret = "secret" } });
            mckTokenService.Setup(x => x.AuthorizationGrantIsValid(It.IsAny<AuthorizationGrantBase>())).Returns(true);
            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.FindClient("321")).Returns(new ClientBase { ClientSecret = "secret", ClientId = "321" });
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);

            
            AuthenticationCodeProcessor processor = new AuthenticationCodeProcessor(mckFactory.Object);
            CommonProcessorErrorAssert(processor, context, Parameters.ErrorParameters.ErrorValues.InvalidGrant);

            mckFactory.VerifyAll();
            mckClientService.VerifyAll();
            mckTokenService.VerifyAll();
        }

    }
}
