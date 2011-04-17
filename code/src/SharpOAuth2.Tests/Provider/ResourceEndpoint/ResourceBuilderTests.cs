using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.ResourceEndpoint;
using System.Collections.Specialized;
using System.Web;
using Moq;

namespace SharpOAuth2.Tests.Provider.ResourceEndpoint
{
    [TestFixture]
    public class ResourceBuilderTests
    {
        [Test]
        public void TestCreatingContextFromUrl()
        {
            ResourceContextBuilder builder = new ResourceContextBuilder();

            IResourceContext context = builder.FromUri("http://wwww.mysite.com?bearer_token=token");

            Assert.AreEqual("token", context.QueryString["bearer_token"]);
            Assert.IsNull(context.Form);
            Assert.IsNull(context.Headers);
        }

        [Test]
        public void TestCreatingContextFromRequest()
        {
            NameValueCollection form = new NameValueCollection();
            NameValueCollection query = new NameValueCollection();
            NameValueCollection headers = new NameValueCollection();

            Mock<HttpRequestBase> mckRequest = new Mock<HttpRequestBase>();
            mckRequest.SetupGet(x => x.Form).Returns(form);
            mckRequest.SetupGet(x => x.QueryString).Returns(query);
            mckRequest.SetupGet(x => x.Headers).Returns(headers);

            ResourceContextBuilder builder = new ResourceContextBuilder();
            IResourceContext context = builder.FromHttpRequest(mckRequest.Object);

            Assert.AreEqual(form, context.Form);
            Assert.AreEqual(query, context.QueryString);
            Assert.AreEqual(headers, context.Headers);
        }

    }
}
