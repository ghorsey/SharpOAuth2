using System;
using Moq;
using NUnit.Framework;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.AuthorizationEndpoint.Processor;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Tests.Provider.AuthorizationEndpoint.Processor
{
    [TestFixture]
    public class ImplicitFlowProcessorTests
    {
        [Test]
        public void TestProcessorSatisfiedBy()
        {
            AuthorizationContext context = new AuthorizationContext();

            context.ResponseType = Parameters.ResponseTypeValues.AccessToken;

            ImplicitFlowProcessor processor = new ImplicitFlowProcessor(new Mock<IServiceFactory> ().Object);
            Assert.IsTrue(processor.IsSatisfiedBy(context));
            context.ResponseType = Parameters.ResponseTypeValues.AuthorizationCode;
            Assert.IsFalse(processor.IsSatisfiedBy(context));
        }


        [Test]
        public void TestProcessTokenRequest()
        {
            AccessTokenBase token = new AccessTokenBase
            {
                Scope = new string[] { "create", "delete" },
                ExpiresIn = 120,
                RefreshToken = "refresh",
                Token = "token",
                TokenType = "bearer"
            };
            ClientBase client = new ClientBase
            {
                ClientId = "123",
                ClientSecret = "secret"
            };

            AuthorizationGrantBase grant = new AuthorizationGrantBase
            {
                Code = "123"
            };

            AuthorizationContext context = new AuthorizationContext
            {
                Client = new ClientBase { ClientId = "123" },
                IsApproved = true,
                RedirectUri = new Uri("http://www.mysite.com/callback"),
                Scope = new string[] { "create", "delete" },
                ResourceOwnerUsername = "owner"
            };

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.FindClient("123")).Returns(client);

            Mock<IAuthorizationGrantService> mckGrantService = new Mock<IAuthorizationGrantService>();
            mckGrantService.Setup(x => x.IssueAuthorizationGrant(context)).Returns(grant);

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.IssueAccessToken(grant)).Returns(token);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);
            mckFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckFactory.SetupGet(x => x.AuthorizationGrantService).Returns(mckGrantService.Object);

            ImplicitFlowProcessor processor = new ImplicitFlowProcessor(mckFactory.Object);
            processor.Process(context);

            Assert.AreEqual(token, context.Token);
            mckClientService.VerifyAll();
            mckFactory.VerifyAll();
            mckTokenService.VerifyAll();            
        }
    }
}
