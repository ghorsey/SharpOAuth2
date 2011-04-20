using System;
using CuttingEdge.ServiceLocation;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using SharpOAuth2.Provider;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Services;
using SharpOAuth2.Provider.TokenEndpoint;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint
{
    [TestFixture]
    public class TokenProviderTests
    {
        [Test]
        public void TestValidContext()
        {
            TokenContext context = new TokenContext
            {
                AuthorizationCode = "auth-code",
                Client = new ClientBase{ ClientId = "client-id", ClientSecret = "secret"},
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                RedirectUri = new Uri( "http://www.mysite.com/callback")
            };

            Mock<IServiceFactory> mckServiceFactory = new Mock<IServiceFactory>();
            Mock<IContextInspector<ITokenContext>> mckInspector = new Mock<IContextInspector<ITokenContext>>();
            
            Mock<ContextProcessor<ITokenContext>> mckProcessor = new Mock<ContextProcessor<ITokenContext>>(mckServiceFactory.Object);
            
            mckProcessor.Setup(x => x.IsSatisfiedBy(context)).Returns(true);

            SimpleServiceLocator locator = new SimpleServiceLocator();
            
            locator.RegisterAll<IContextInspector<ITokenContext>>(mckInspector.Object);
            locator.RegisterAll<ContextProcessor<ITokenContext>>(mckProcessor.Object);
            ServiceLocator.SetLocatorProvider(() => locator);

            TokenProvider provider = new TokenProvider();

            provider.GrantAccessToken(context);
        }


        [Test]
        public void TestUnhandledGrantTypeContext()
        {
            TokenContext context = new TokenContext
            {
                AuthorizationCode = "auth-code",
                Client = new ClientBase { ClientId = "client-id", ClientSecret = "secret" },
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                RedirectUri = new Uri("http://www.mysite.com/callback")
            };

            Mock<IServiceFactory> mckServiceFactory = new Mock<IServiceFactory>();
            Mock<IContextInspector<ITokenContext>> mckInspector = new Mock<IContextInspector<ITokenContext>>();
            Mock<ContextProcessor<ITokenContext>> mckUnsupportedProcessor = new Mock<ContextProcessor<ITokenContext>>(mckServiceFactory.Object);
            mckUnsupportedProcessor.Setup(x => x.IsSatisfiedBy(context)).Returns(false);


            SimpleServiceLocator locator = new SimpleServiceLocator();
            locator.RegisterAll<ContextProcessor<ITokenContext>>(mckUnsupportedProcessor.Object);
            locator.RegisterAll<IContextInspector<ITokenContext>>(mckInspector.Object);
            ServiceLocator.SetLocatorProvider(() => locator);

            TokenProvider provider = new TokenProvider();

            provider.GrantAccessToken(context);
        }
    }
}
