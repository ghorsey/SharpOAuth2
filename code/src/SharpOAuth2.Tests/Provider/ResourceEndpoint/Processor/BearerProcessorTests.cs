using System.Collections.Specialized;
using Moq;
using NUnit.Framework;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.ResourceEndpoint;
using SharpOAuth2.Provider.ResourceEndpoint.Processors;
using SharpOAuth2.Provider.Services;

namespace SharpOAuth2.Tests.Provider.ResourceEndpoint.Processor
{
    [TestFixture]
    public class BearerProcessorTests
    {
        [Test]
        public void TestIsSatisfiedByQueryStringTrue()
        {
            ResourceContext context = new ResourceContext();
            context.QueryString = new NameValueCollection();
            context.QueryString["bearer_token"] = "12345";

            BearerProcessor processor = new BearerProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsTrue(processor.IsSatisfiedBy(context));

        }

        [Test]
        public void TestIsSatisfiedByFormTrue()
        {
            ResourceContext context = new ResourceContext();
            context.Form = new NameValueCollection();
            context.Form["bearer_token"] = "12345";

            BearerProcessor processor = new BearerProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsTrue(processor.IsSatisfiedBy(context));
        }

        [Test]
        public void TestIsSatisfiedByHeaderTrue()
        {
            ResourceContext context = new ResourceContext();
            context.Headers = new NameValueCollection();
            context.Headers["Authorization"] = "Bearer 123456";

            BearerProcessor processor = new BearerProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsTrue(processor.IsSatisfiedBy(context));
        }


        [Test]
        public void TestIsSatisfiedFalse()
        {
            ResourceContext context = new ResourceContext();
            BearerProcessor processor = new BearerProcessor(new Mock<IServiceFactory>().Object);

            Assert.IsFalse(processor.IsSatisfiedBy(context));
        }

        [Test]
        public void TestIsSatisfiedFalseWrongAuthorizaitonHeader()
        {
            ResourceContext context = new ResourceContext();
            context.Headers = new NameValueCollection();
            context.Headers["Authorization"] = "MAC token=\"something\",...";
            BearerProcessor processor = new BearerProcessor(new Mock<IServiceFactory>().Object);
            
            Assert.IsFalse(processor.IsSatisfiedBy(context));
        }

        [Test]
        public void TestProcessingContext()
        {
            ResourceContext context = new ResourceContext();
            context.Headers = new NameValueCollection();
            context.Headers["Authorization"] = "BEARER my-token";

            AccessTokenBase result = new AccessTokenBase() { Token = "my-token" };
            Mock<ITokenService> mckTokenService = new Mock<ITokenService>();
            mckTokenService.Setup(x => x.FindToken("my-token")).Returns(result);

            Mock<IServiceFactory> mckFactory = new Mock<IServiceFactory>();
            mckFactory.SetupGet(x => x.TokenService).Returns(mckTokenService.Object);

            BearerProcessor processor = new BearerProcessor(mckFactory.Object);

            processor.Process(context);

            Assert.AreEqual(result, context.Token);

            mckTokenService.VerifyAll();
            mckFactory.VerifyAll();

        }
    }
}
