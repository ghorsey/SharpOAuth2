using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.Authorization;
using Moq;
using SharpOAuth2.Provider.Authorization.Services;
using Microsoft.Practices.ServiceLocation;
using CuttingEdge.ServiceLocation;
using SharpOAuth2.Provider.Authorization.Inspectors;
using SharpOAuth2.Provider;

namespace SharpOAuth2.Tests.Provider.Authorization
{
    [TestFixture]
    public class AuthorizationProviderTests
    {
        private static AuthorizationGrantBase MakeCommonToken(IAuthorizationContext context)
        {
            return new AuthorizationGrantBase { Expires = 3000, Scope = context.Scope, Token = new Guid().ToString() };
        }
        private static Mock<IClientService> MakeClientService(IAuthorizationContext context, bool validClient, bool validUri)
        {
            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.IsClient(context)).Returns(validClient);
            mckClientService.Setup(x => x.ValidateRedirectUri(context)).Returns(validUri);
            return mckClientService;
        }
        private static Mock<IAuthorizationServiceFactory> MakeServiceFactory(Mock<IClientService> clientService, Mock<ITokenService> tokenService)
        {
            Mock<IAuthorizationServiceFactory> mckFactory = new Mock<IAuthorizationServiceFactory>();
            mckFactory.SetupGet(x => x.ClientService).Returns(clientService.Object);
            mckFactory.SetupGet(x => x.TokenService).Returns(tokenService.Object);
            return mckFactory;
        }
        private static Mock<ITokenService> MakeTokenService(IAuthorizationContext context, AuthorizationGrantBase token)
        {
            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.MakeAuthorizationGrant(context)).Returns(token);
            return mckTokenService;
        }
        private static Mock<ITokenService> MakeTokenService(IAuthorizationContext context)
        {
            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.MakeAuthorizationGrant(context)).Returns(MakeCommonToken(context));
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
        [SetUp]
        public void SetUp()
        {
            SimpleServiceLocator container = new SimpleServiceLocator();
            container.RegisterAll<IAuthorizationContextInspector>(new SpecificationInspector());

            ServiceLocator.SetLocatorProvider(() => container);
        }
        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestCreatingAuthorizationRequestFatalAuthorizationExists()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext ();
            context.Authorization = new AccessTokenBase();
            
            IAuthorizationProvider provider = new AuthorizationProvider(new Mock<IAuthorizationServiceFactory>().Object);

            provider.CreateAuthorizationGrant(context, true);

        }
        [Test]
        public void TestCreatingAuthorizationRequest()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();

            Mock<IClientService> mckClientService = MakeClientService(context, true, true);
            Mock<ITokenService> mckTokenService = MakeTokenService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(MakeServiceFactory(mckClientService, mckTokenService).Object);

            provider.CreateAuthorizationGrant(context, true);

            Assert.IsNotNull(context.Authorization);
            Assert.IsTrue(context.IsApproved);
            mckTokenService.VerifyAll();
            mckClientService.VerifyAll();
        }

        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestCreatingAuthorizationRequestInvalidClient()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();

            Mock<IClientService> mckClientService = MakeClientService(context, false, true);

            Mock<ITokenService> mckTokenService = MakeTokenService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(MakeServiceFactory(mckClientService, mckTokenService).Object);

            provider.CreateAuthorizationGrant(context, true);
        }

        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestCreatingAuthorizationRequestInvalidUri()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();

            Mock<IClientService> mckClientService = MakeClientService(context, true, false);

            Mock<ITokenService> mckTokenService = MakeTokenService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(MakeServiceFactory(mckClientService, mckTokenService).Object);

            provider.CreateAuthorizationGrant(context, true);
        }


        [Test]
        public void TestCreatingAuthorizationRequestWithInputErrors()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();
            context.ResponseType = "unk";

            Mock<IClientService> mckClientService = MakeClientService(context, true, true);

            Mock<ITokenService> mckTokenService = MakeTokenService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(MakeServiceFactory(mckClientService, mckTokenService).Object);

            provider.CreateAuthorizationGrant(context, true);

            Assert.IsNull(context.Authorization);
            Assert.IsFalse(context.IsApproved);
            Assert.IsNotNull(context.Error);
            Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.UnsupportedResponseType, context.Error.Error);

        }
    }
}
