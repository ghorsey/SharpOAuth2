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
    public class ResourceOwnerPasswordCredentialProcessorTests
    {
        [Test]
        public void TestProcessorIsSatisified()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.Password;

            ResourceOwnerPasswordCredentialProcessor processor = new ResourceOwnerPasswordCredentialProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsTrue(processor.IsSatisfiedBy(context));
        }

        [Test]
        public void TestProcessorIsNotSatisified()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.AuthorizationCode;

            ResourceOwnerPasswordCredentialProcessor processor = new ResourceOwnerPasswordCredentialProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsFalse(processor.IsSatisfiedBy(context));
        }

        [Test]
        public void TestIssueingAccessToken()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.Password;

            context.Client = new ClientBase
            {
                ClientId = "123",
                ClientSecret = "secret"
            };
            context.ResourceOwnerPassword = "password";
            context.ResourceOwnerUsername = "username";

            AccessTokenBase token = new AccessTokenBase();
            Mock<IResourceOwnerService> mckResourceOwnerService = new Mock<IResourceOwnerService>();
            mckResourceOwnerService.Setup(x => x.CredentialsAreValid(context)).Returns(true);

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);

            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.IssueAccessTokenForResourceOwner(context)).Returns(token);

            Mock<IServiceFactory> mckServicFactory = new Mock<IServiceFactory>();
            mckServicFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckServicFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);
            mckServicFactory.SetupGet(x => x.ResourceOwnerService).Returns(mckResourceOwnerService.Object);

            ResourceOwnerPasswordCredentialProcessor processor = new ResourceOwnerPasswordCredentialProcessor(mckServicFactory.Object);

            processor.Process(context);

            Assert.AreEqual(token, context.Token);
            mckClientService.VerifyAll();
            mckResourceOwnerService.VerifyAll();
            mckServicFactory.VerifyAll();
            mckTokenService.VerifyAll();

                 
        }

        [Test]
        public void TestIssueingAccessTokenInvalidResourceOwner()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.Password;

            context.Client = new ClientBase
            {
                ClientId = "123",
                ClientSecret = "secret"
            };
            context.ResourceOwnerPassword = "password";
            context.ResourceOwnerUsername = "username";

            Mock<IResourceOwnerService> mckResourceOwnerService = new Mock<IResourceOwnerService>();
            mckResourceOwnerService.Setup(x => x.CredentialsAreValid(context)).Returns(false);

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(true);

            Mock<IServiceFactory> mckServicFactory = new Mock<IServiceFactory>();
            mckServicFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            mckServicFactory.SetupGet(x => x.ResourceOwnerService).Returns(mckResourceOwnerService.Object);

            ResourceOwnerPasswordCredentialProcessor processor = new ResourceOwnerPasswordCredentialProcessor(mckServicFactory.Object);

            AssertExceptionResponse(context, processor, Parameters.ErrorParameters.ErrorValues.InvalidRequest, 400);
                        
            mckClientService.VerifyAll();
            mckResourceOwnerService.VerifyAll();
            mckServicFactory.VerifyAll();
        }
        [Test]
        public void TestIssueingAccessTokenInvalidClient()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.Password;

            context.Client = new ClientBase
            {
                ClientId = "123",
                ClientSecret = "secret"
            };
            context.ResourceOwnerPassword = "password";
            context.ResourceOwnerUsername = "username";

            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(context)).Returns(false);

            Mock<IServiceFactory> mckServicFactory = new Mock<IServiceFactory>();
            mckServicFactory.SetupGet(x => x.ClientService).Returns(mckClientService.Object);
            
            ResourceOwnerPasswordCredentialProcessor processor = new ResourceOwnerPasswordCredentialProcessor(mckServicFactory.Object);
            AssertExceptionResponse(context, processor, Parameters.ErrorParameters.ErrorValues.InvalidClient, 401);
            mckClientService.VerifyAll();
            mckServicFactory.VerifyAll();
        }

        private static void AssertExceptionResponse(TokenContext context, ResourceOwnerPasswordCredentialProcessor processor, string parameter, int httpStatusCode)
        {
            try
            {
                processor.Process(context);
                Assert.Fail("No exception was thrown");
            }
            catch (OAuthErrorResponseException<ITokenContext> x)
            {
                Assert.AreEqual(parameter, x.Error);
                Assert.AreEqual(httpStatusCode, x.HttpStatusCode);
            }
            catch (OAuthErrorResponseException<IOAuthContext> x)
            {
                Assert.AreEqual(parameter, x.Error);
                Assert.AreEqual(httpStatusCode, x.HttpStatusCode);
            }
            catch (Exception x)
            {
                Assert.Fail("Unexpected exception was thrown: " + x.Message);
            }
        }
    }
}
