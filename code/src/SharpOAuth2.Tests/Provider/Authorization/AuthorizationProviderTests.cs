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
        private static IToken MakeCommonToken(IAuthorizationContext context)
        {
            return new TokenBase { Expires = 3000, Scope = context.Scope, Token = new Guid().ToString() };
        }
        private static Mock<IClientService> MakeClientService(IAuthorizationContext context, bool validClient, bool validUri)
        {
            Mock<IClientService> mckClientService = new Mock<IClientService>();
            mckClientService.Setup(x => x.AuthenticateClient(It.IsAny<IClient>())).Returns(validClient);
            mckClientService.Setup(x => x.ValidateRedirectUri(context)).Returns(validUri);
            return mckClientService;
        }
        private static Mock<ITokenService> MakeTokenService(IAuthorizationContext context, IToken token)
        {
            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.MakeRequestToken(context)).Returns(token);
            return mckTokenService;
        }
        private static Mock<ITokenService> MakeTokenService(IAuthorizationContext context)
        {
            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.MakeRequestToken(context)).Returns(MakeCommonToken(context));
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
            context.Authorization = new TokenBase();
            
            IAuthorizationProvider provider = new AuthorizationProvider(new Mock<IClientService>().Object, new Mock<ITokenService>().Object);

            provider.CreateAuthorizationRequest(context);

        }
        [Test]
        public void TestCreatingAuthorizationRequest()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();

            Mock<IClientService> mckClientService = MakeClientService(context, true, true);

            Mock<ITokenService> mckTokenService = MakeTokenService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(mckClientService.Object, mckTokenService.Object);

            provider.CreateAuthorizationRequest(context);

            Assert.IsNotNull(context.Authorization);
            Assert.IsFalse(context.IsApproved);
            mckTokenService.VerifyAll();
            mckClientService.VerifyAll();
        }

        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestCreatingAuthorizationRequestInvalidClient()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();

            Mock<IClientService> mckClientService = MakeClientService(context, false, true);

            Mock<ITokenService> mckTokenService = MakeTokenService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(mckClientService.Object, mckTokenService.Object);

            provider.CreateAuthorizationRequest(context);
        }

        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestCreatingAuthorizationRequestInvalidUri()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();

            Mock<IClientService> mckClientService = MakeClientService(context, true, false);

            Mock<ITokenService> mckTokenService = MakeTokenService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(mckClientService.Object, mckTokenService.Object);

            provider.CreateAuthorizationRequest(context);
        }


        [Test]
        public void TestCreatingAuthorizationRequestWithInputErrors()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();
            context.ResponseType = "unk";

            Mock<IClientService> mckClientService = MakeClientService(context, true, true);

            Mock<ITokenService> mckTokenService = MakeTokenService(context);

            IAuthorizationProvider provider = new AuthorizationProvider(mckClientService.Object, mckTokenService.Object);

            provider.CreateAuthorizationRequest(context);

            Assert.IsNotNull(context.Authorization);
            Assert.IsFalse(context.IsApproved);
            Assert.IsNotNull(context.Error);
            Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.UnsupportedResponseType, context.Error.Error);

        }

        [Test]
        public void TestApproveAuthorization()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();
            IToken t = MakeCommonToken(context);
            context.Authorization = t;
            Mock<IClientService> mckClientService = MakeClientService(context, true, true);
            Mock<ITokenService> mckTokenService = MakeTokenService(context, t);
            mckTokenService.Setup(x => x.ApproveAuthorizationToken(t));

            IAuthorizationProvider provider = new AuthorizationProvider(mckClientService.Object, mckTokenService.Object);

            provider.ApproveAuthorizationRequest(context);

            Assert.IsTrue(context.IsApproved);
            mckTokenService.Verify(x => x.ApproveAuthorizationToken(t));
        }

        [Test]
        public void TestDenyAuthorization()
        {
            AuthorizationContext context = MakeCommonAuthorizationContext();
            context.IsApproved = true;
            IToken t = MakeCommonToken(context);
            context.Authorization = t;
            Mock<IClientService> mckClientService = MakeClientService(context, true, true);
            Mock<ITokenService> mckTokenService = MakeTokenService(context, t);
            mckTokenService.Setup(x => x.DenyAuthorizationToken(t));

            IAuthorizationProvider provider = new AuthorizationProvider(mckClientService.Object, mckTokenService.Object);

            provider.DenyAuthorizationRequest(context);

            Assert.IsFalse(context.IsApproved);
            mckTokenService.Verify(x => x.DenyAuthorizationToken(t));
        }
    }
}
