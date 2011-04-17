using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.ResourceEndpoint;
using System.Collections.Specialized;
using SharpOAuth2.Provider;
using Moq;
using CuttingEdge.ServiceLocation;
using Microsoft.Practices.ServiceLocation;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Tests.Provider.ResourceEndpoint
{
    [TestFixture]
    public class ResourceProviderTests
    {
        [Test]
        public void TestAccessingProtectedResource()
        {
            ResourceContext context = new ResourceContext();
            context.Headers = new NameValueCollection();
            context.Headers["Authorization"] = "bearer my-token";

            Mock<ContextProcessor<IResourceContext>> mckProvider = new Mock<ContextProcessor<IResourceContext>>(new Mock<IServiceFactory>().Object);
            mckProvider.Setup(x => x.IsSatisfiedBy(context)).Returns(true);
            mckProvider.Setup(x => x.Process(context)).Callback(() => { context.Token = new AccessTokenBase(); });

            SimpleServiceLocator container = new SimpleServiceLocator();
            container.RegisterAll<ContextProcessor<IResourceContext>>(mckProvider.Object);

            ServiceLocator.SetLocatorProvider(() => container);

            ResourceProvider provider = new ResourceProvider();

            provider.AccessProtectedResource(context);


            mckProvider.Verify();

        }


        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestAccessingResourceUnhandledByProcessor()
        {
            ResourceContext context = new ResourceContext();
            context.Headers = new NameValueCollection();
            context.Headers["Authorization"] = "bearer my-token";

            Mock<ContextProcessor<IResourceContext>> mckProvider = new Mock<ContextProcessor<IResourceContext>>(new Mock<IServiceFactory>().Object);
            mckProvider.Setup(x => x.IsSatisfiedBy(context)).Returns(false);

            SimpleServiceLocator container = new SimpleServiceLocator();
            container.RegisterAll<ContextProcessor<IResourceContext>>(mckProvider.Object);

            ServiceLocator.SetLocatorProvider(() => container);

            ResourceProvider provider = new ResourceProvider();

            provider.AccessProtectedResource(context);


            mckProvider.Verify();

        }

        [Test]
        public void TestAccessingNullTokenResource()
        {
            ResourceContext context = new ResourceContext();
            context.Headers = new NameValueCollection();
            context.Headers["Authorization"] = "bearer my-token";

            Mock<ContextProcessor<IResourceContext>> mckProvider = new Mock<ContextProcessor<IResourceContext>>(new Mock<IServiceFactory>().Object);
            mckProvider.Setup(x => x.IsSatisfiedBy(context)).Returns(true);
            mckProvider.Setup(x => x.Process(context));

            SimpleServiceLocator container = new SimpleServiceLocator();
            container.RegisterAll<ContextProcessor<IResourceContext>>(mckProvider.Object);

            ServiceLocator.SetLocatorProvider(() => container);

            ResourceProvider provider = new ResourceProvider();

            CommonErrorAssert(context, provider, Parameters.ErrorParameters.ErrorValues.InvalidToken);

            mckProvider.Verify();
        }

        [Test]
        public void TestAccessingExpiredTokenResource()
        {
            ResourceContext context = new ResourceContext();
            context.Headers = new NameValueCollection();
            context.Headers["Authorization"] = "bearer my-token";

            AccessTokenBase expiredToken = new AccessTokenBase
            {
                Token = "my-token",
                ExpiresIn = 2,
                IssuedOn = DateTime.Now.AddMinutes(-1).ToEpoch()
            };
            Mock<ContextProcessor<IResourceContext>> mckProvider = new Mock<ContextProcessor<IResourceContext>>(new Mock<IServiceFactory>().Object);
            mckProvider.Setup(x => x.IsSatisfiedBy(context)).Returns(true);
            mckProvider.Setup(x => x.Process(context)).Callback(() => context.Token = expiredToken);

            SimpleServiceLocator container = new SimpleServiceLocator();
            container.RegisterAll<ContextProcessor<IResourceContext>>(mckProvider.Object);

            ServiceLocator.SetLocatorProvider(() => container);

            ResourceProvider provider = new ResourceProvider();

            CommonErrorAssert(context, provider, Parameters.ErrorParameters.ErrorValues.InvalidToken);

            mckProvider.Verify();
        }



        private static void CommonErrorAssert(ResourceContext context, ResourceProvider provider, string error)
        {
            try
            {
                provider.AccessProtectedResource(context);
                Assert.Fail("No exception was thrown");
            }
            catch (OAuthErrorResponseException<IResourceContext> x)
            {
                Assert.AreEqual(error, x.Error);
            }
            catch (Exception x)
            {
                Assert.Fail("Unexpected exception was thrown:" + x.Message);
            }
        }
    }
}
