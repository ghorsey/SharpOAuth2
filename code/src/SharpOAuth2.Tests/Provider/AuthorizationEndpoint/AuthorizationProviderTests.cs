using System;
using CuttingEdge.ServiceLocation;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.AuthorizationEndpoint.Inspectors;
using SharpOAuth2.Provider.AuthorizationEndpoint.Processor;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Tests.Provider.AuthorizationEndpoint
{
    [TestFixture]
    public class AuthorizationProviderTests
    {
        private static AuthorizationGrantBase MakeCommonToken(IAuthorizationContext context)
        {
            return new AuthorizationGrantBase { Code = new Guid().ToString() };
        }
        private static Mock<IClientService> MakeClientService(IAuthorizationContext context, bool validClient, bool validUri)
        {
            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.IsClient(context)).Returns(validClient);
            mckClientService.Setup(x => x.ValidateRedirectUri(context)).Returns(validUri);
            return mckClientService;
        }
        private static Mock<IServiceFactory> MakeServiceFactory(Mock<IClientService> clientService, Mock<IAuthorizationGrantService> grantService)
        {
            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(clientService.Object);
            mckFactory.SetupGet(x => x.AuthorizationGrantService).Returns(grantService.Object);
            return mckFactory;
        }
        private static Mock<IAuthorizationGrantService> MakeGrantService(IAuthorizationContext context, AuthorizationGrantBase token)
        {
            Mock<IAuthorizationGrantService> mckTokenService = new Mock<IAuthorizationGrantService>();
            mckTokenService.Setup(x => x.IssueAuthorizationGrant(context)).Returns(token);
            return mckTokenService;
        }
        private static Mock<IAuthorizationGrantService> MakeGrantService(IAuthorizationContext context)
        {
            Mock<IAuthorizationGrantService> mckTokenService = new Mock<IAuthorizationGrantService>();
            mckTokenService.Setup(x => x.IssueAuthorizationGrant(context)).Returns(MakeCommonToken(context));
            return mckTokenService;
        }
        private static AuthorizationContext MakeCommonAuthorizationContext()
        {
            AuthorizationContext context = new AuthorizationContext();
            context.Client = new ClientBase { ClientId = "id", ClientSecret = "secret" };
            context.RedirectUri = new Uri("http://www.mysite.com/callback");
            context.ResponseType = Parameters.ResponseTypeValues.AuthorizationCode;
            context.Scope = new string[] { "some", "scope" };
            context.State = "my state";
            return context;
        }
        
        public void SetUp(IServiceFactory factory)
        {
            SimpleServiceLocator container = new SimpleServiceLocator();
            container.RegisterAll<IContextInspector<IAuthorizationContext>>(new SpecificationInspector());
            container.RegisterAll<ContextProcessor<IAuthorizationContext>>(new AuthorizationCodeProcessor(factory));
            ServiceLocator.SetLocatorProvider(() => container);
        }
        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestCreatingAuthorizationRequestFatalAuthorizationExists()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext ();
            context.Token = new AccessTokenBase();
            
            IAuthorizationProvider provider = new AuthorizationProvider(new Mock<IServiceFactory>().Object);

            provider.CreateAuthorizationGrant(context);

        }
        [Test]
        public void TestCreatingAuthorizationRequest()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();
            context.IsApproved = true;
            context.ResourceOwnerUsername = "1234";
            Mock<IClientService> mckClientService = MakeClientService(context, true, true);
            Mock<IAuthorizationGrantService> mckGrantService = MakeGrantService(context);
            Mock<IServiceFactory> mckServiceFactory = MakeServiceFactory(mckClientService, mckGrantService);

            SetUp(mckServiceFactory.Object);


            IAuthorizationProvider provider = new AuthorizationProvider(mckServiceFactory.Object);

            provider.CreateAuthorizationGrant(context);

            Assert.IsNotNull(context.AuthorizationGrant);
            Assert.IsTrue(context.IsApproved);
            mckGrantService.VerifyAll();
            mckClientService.VerifyAll();
        }

        [Test]
        public void TestCreatingUnauthorizedAuthorizationRequest()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();
            context.IsApproved = false;
            context.ResourceOwnerUsername = "12345";
            Mock<IClientService> mckClientService = MakeClientService(context, true, true);
            Mock<IAuthorizationGrantService> mckAuthService = MakeGrantService(context);
            Mock<IServiceFactory> mckServiceFactory = MakeServiceFactory(mckClientService, mckAuthService);

            SetUp(mckServiceFactory.Object);

            IAuthorizationProvider provider = new AuthorizationProvider(mckServiceFactory.Object);

            provider.CreateAuthorizationGrant(context);

            Assert.IsNotNull(context.AuthorizationGrant);
            Assert.IsFalse(context.IsApproved);
            Assert.IsNotNull(context.Error);
            Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.AccessDenied, context.Error.Error);
            mckAuthService.VerifyAll();
            mckClientService.VerifyAll();
        }

        [Test]
        public void TestCreatingAuthorizationRequestInvalidClient()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();

            Mock<IClientService> mckClientService = MakeClientService(context, false, true);

            Mock<IAuthorizationGrantService> mckAuthService = MakeGrantService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(MakeServiceFactory(mckClientService, mckAuthService).Object);

            provider.CreateAuthorizationGrant(context);

            Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.UnauthorizedClient, context.Error.Error);
        }

        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestCreatingAuthorizationWithoutResourceOwner()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();

            Mock<IClientService> mckClientService = MakeClientService(context, true, true);

            Mock<IAuthorizationGrantService> mckAuthService = MakeGrantService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(MakeServiceFactory(mckClientService, mckAuthService).Object);

            provider.CreateAuthorizationGrant(context);
        }

        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestCreatingAuthorizationRequestInvalidUri()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();

            Mock<IClientService> mckClientService = MakeClientService(context, true, false);

            Mock<IAuthorizationGrantService> mckAuthService = MakeGrantService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(MakeServiceFactory(mckClientService, mckAuthService).Object);

            provider.CreateAuthorizationGrant(context);
        }


        [Test]
        public void TestCreatingAuthorizationRequestWithInputErrors()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();
            context.ResponseType = "unk";
            context.ResourceOwnerUsername = "owner-id";

            Mock<IClientService> mckClientService = MakeClientService(context, true, true);

            Mock<IAuthorizationGrantService> mckAuthService = MakeGrantService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(MakeServiceFactory(mckClientService, mckAuthService).Object);

            provider.CreateAuthorizationGrant(context);

            Assert.IsNull(context.Token);
            Assert.IsFalse(context.IsApproved);
            Assert.IsNotNull(context.Error);
            Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.UnsupportedResponseType, context.Error.Error);

        }

        [Test]
        public void TestCreatingAuthorizationRequestImplicitFlow()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();
            context.ResourceOwnerUsername = "1234";
            context.ResponseType = Parameters.ResponseTypeValues.AccessToken;

            Mock<IClientService> mckClientService = MakeClientService(context, true, true);

            Mock<IAuthorizationGrantService> mckAuthService = MakeGrantService(context);
            Mock<IServiceFactory> mckServiceFactory = MakeServiceFactory(mckClientService, mckAuthService);

            SetUp(mckServiceFactory.Object);

            IAuthorizationProvider provider = new AuthorizationProvider(mckServiceFactory.Object);

            provider.CreateAuthorizationGrant(context);

            Assert.IsNull(context.Token);
            Assert.IsFalse(context.IsApproved);
            Assert.IsNotNull(context.Error);
            Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.UnsupportedResponseType, context.Error.Error);

        }
        [Test]
        public void TestCheckingIfOwnerAlreadyApprovedClient()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();
            context.ResourceOwnerUsername = "1234";


            Mock<IClientService> mckClientService = MakeClientService(context, true, true);
            mckClientService.Setup(x=>x.IsAccessGranted(context.Client, context.Scope, context.ResourceOwnerUsername)).Returns(true);
            Mock<IAuthorizationGrantService> mckAuthService = MakeGrantService(context);
            Mock<IServiceFactory> mckServiceFactory = MakeServiceFactory(mckClientService, mckAuthService);

            SetUp(mckServiceFactory.Object);

            IAuthorizationProvider provider = new AuthorizationProvider(mckServiceFactory.Object);

            Assert.IsTrue(provider.IsAccessApproved(context));

        }

    }
}
