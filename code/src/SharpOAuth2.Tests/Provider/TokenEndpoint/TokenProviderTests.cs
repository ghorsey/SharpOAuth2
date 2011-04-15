using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.TokenEndpoint;
using CuttingEdge.ServiceLocation;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider;
using Moq;
using SharpOAuth2.Provider.Services;

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

            provider.GrantAuthorizationToken(context);
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

            provider.GrantAuthorizationToken(context);
        }
    }
}
