using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider;
using SharpOAuth2.Provider.Services;
using NUnit.Framework;
using SharpOAuth2.Provider.TokenEndpoint.Processor;
using Moq;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Processors
{
    [TestFixture]
    public class ClientCredentialsProcessorTests 
    {
        [Test]
        public void TestIsNotSatisfiedByAuthorizationCodeGrant()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.AuthorizationCode;

            ClientCredentialsProcessor processor = new ClientCredentialsProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsFalse(processor.IsSatisfiedBy(context));
        }

        [Test]
        public void TestIsSatisfiedByClientCredentialsGrant()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.ClientCredentials;

            ClientCredentialsProcessor processor = new ClientCredentialsProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsTrue(processor.IsSatisfiedBy(context));
        }

        [Test]
        public void IssueAccessTokenForClientCredentials()
        {
            ClientBase cl = new ClientBase { ClientId = "123", ClientSecret = "secret" };
            AccessTokenBase token = new AccessTokenBase{ ExpiresIn = 120, Token = Guid.NewGuid().ToString()};

            TokenContext context = new TokenContext
            {
                Client = new ClientBase { ClientId = "123", ClientSecret = "secret" },
                GrantType = Parameters.GrantTypeValues.ClientCredentials
            };

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x=>x.AuthenticateClient(context)).Returns(true);
            mckClientService.Setup(x => x.FindClient("123")).Returns(cl);
            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x=>x.MakeAccessToken(cl)).Returns(token);
            Mock<IServiceFactory> mckServiceFactory = new Mock<IServiceFactory>();
            mckServiceFactory.SetupGet(x=>x.ClientService).Returns(mckClientService.Object);
            mckServiceFactory.SetupGet(x=>x.TokenService).Returns(mckTokenService.Object);

            ClientCredentialsProcessor processor = new ClientCredentialsProcessor(mckServiceFactory.Object);

            processor.Process(context);

            mckClientService.VerifyAll();
            mckServiceFactory.VerifyAll();
            mckTokenService.VerifyAll();

            Assert.AreEqual(token, context.Token);
        }

        [Test]
        public void IssueAccessTokenForInvalidClientCredentials()
        {
            ClientBase cl = new ClientBase { ClientId = "123", ClientSecret = "secret" };
            AccessTokenBase token = new AccessTokenBase { ExpiresIn = 120, Token = Guid.NewGuid().ToString() };

            TokenContext context = new TokenContext
            {
                Client = cl,
                GrantType = Parameters.GrantTypeValues.ClientCredentials
            };

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(false);
            Mock<IServiceFactory> mckServiceFactory = new Mock<IServiceFactory>();
            mckServiceFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);


            ClientCredentialsProcessor processor = new ClientCredentialsProcessor(mckServiceFactory.Object);

            try
            {

                processor.Process(context);
                Assert.Fail("no exception thrown");
            }
            catch (OAuthErrorResponseException<ITokenContext> x)
            {
                Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.UnauthorizedClient, x.Error);
                Assert.AreEqual(400, x.HttpStatusCode);
            }
            catch (Exception x)
            {
                Assert.Fail("unexpected exception was thrown: " + x.Message);
            }

            mckClientService.VerifyAll();
            mckServiceFactory.VerifyAll();
        }
    }
}
