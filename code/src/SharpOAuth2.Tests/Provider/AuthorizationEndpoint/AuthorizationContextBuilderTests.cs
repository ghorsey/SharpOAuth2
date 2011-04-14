using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using Moq;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using SharpOAuth2.Provider;

namespace SharpOAuth2.Tests.Provider.AuthorizationEndpoint
{
    [TestFixture]
    public class AuthorizationContextBuilderTests
    {
        private string CreateUrl(string responseType, string clientId, string clientSecret, string scope, string state, string redirectUri)
        {
            string uriFormat = "http://www.oauth.net/authorize?response_type={0}&client_id={1}&client_secret={2}&scope={3}&state={4}&redirect_uri={5}";

            return string.Format(uriFormat,
                HttpUtility.UrlEncode(responseType),
                HttpUtility.UrlEncode(clientId),
                HttpUtility.UrlEncode(clientSecret),
                HttpUtility.UrlEncode(scope),
                HttpUtility.UrlEncode(state),
                HttpUtility.UrlEncode(redirectUri)
            );
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateContextFromNullUriString()
        {
            IContextBuilder<IAuthorizationContext> builder = new AuthorizationContextBuilder();
            builder.FromUri((string)null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void CreateContextFromNullUri()
        {
            IContextBuilder<IAuthorizationContext> builder = new AuthorizationContextBuilder();
            builder.FromUri((Uri)null);
        }

        [Test]
        public void CreateContextFromUri()
        {
            string url = CreateUrl("authorization_code", "1234432", "secret", "create-user manage-clients", "mystate","http://www.mysite.com/callback");

            IContextBuilder<IAuthorizationContext> builder = new AuthorizationContextBuilder();
            IAuthorizationContext context = builder.FromUri(url);

            Assert.AreEqual("authorization_code", context.ResponseType);
            Assert.AreEqual("1234432", context.Client.ClientId);
            Assert.AreEqual("secret", context.Client.ClientSecret);
            Assert.AreEqual(new string[] { "create-user", "manage-clients" }, context.Scope);
            Assert.AreEqual(new Uri("http://www.mysite.com/callback"), context.RedirectUri);
        }

        [Test, ExpectedException(typeof(UriFormatException))]
        public void TestInvalidUriFormat()
        {
            IContextBuilder<IAuthorizationContext> builder = new AuthorizationContextBuilder();
            builder.FromUri("/relativeUrl");
        }

        [Test, ExpectedException(typeof(UriFormatException))]
        public void TestInvalidRedirectUriFormat()
        {
            IContextBuilder<IAuthorizationContext> builder = new AuthorizationContextBuilder();
            string url = CreateUrl("authorization_code", "1234432", "secret", "create-user manage-clients", "mystate","/callback");
            builder.FromUri(url);
        }

        [Test]
        public void CreateContextFromUriWithoutScope()
        {
            string url = CreateUrl("", "", "", null, "", null);

            IContextBuilder<IAuthorizationContext> builder = new AuthorizationContextBuilder();
            IAuthorizationContext context = builder.FromUri(url);

            Assert.IsNull(context.RedirectUri);
            Assert.AreEqual(new string[0], context.Scope); 
        }

        private static void CommonHttpRequestTest(string httpMethod)
        {
            Mock<HttpRequestBase> mckHttpRequest = new Mock<HttpRequestBase>();

            mckHttpRequest.SetupGet(x => x.HttpMethod).Returns(httpMethod);
            NameValueCollection querystring = new NameValueCollection();
            querystring[Parameters.ClientId] = "123";
            querystring[Parameters.ClientSecret] = "secret";
            querystring[Parameters.ResponseType] = Parameters.ResponseTypeValues.AuthorizationCode;
            querystring[Parameters.State] = "state";
            querystring[Parameters.Scope] = "create-member view-clients";
            querystring[Parameters.RedirectUri] = "http://www.mysite.com/callback";
            if (httpMethod.ToUpperInvariant() == "GET")
                mckHttpRequest.SetupGet(x => x.QueryString).Returns(querystring);
            else if (httpMethod.ToUpperInvariant() == "POST")
                mckHttpRequest.SetupGet(x => x.Form).Returns(querystring);

            IContextBuilder<IAuthorizationContext> builder = new AuthorizationContextBuilder();
            IAuthorizationContext context = builder.FromHttpRequest(mckHttpRequest.Object);

            Assert.AreEqual("123", context.Client.ClientId);
            Assert.AreEqual("secret", context.Client.ClientSecret);
            Assert.AreEqual(Parameters.ResponseTypeValues.AuthorizationCode, context.ResponseType);
            Assert.AreEqual("state", context.State);
            Assert.AreEqual(new Uri("http://www.mysite.com/callback", UriKind.Absolute), context.RedirectUri);
            Assert.AreEqual(new string[] { "create-member", "view-clients" }, context.Scope);
        }

        [Test]
        public void CreateContextFromHttpRequest()
        {
            CommonHttpRequestTest("GET");
            CommonHttpRequestTest("POST");
            try
            {
                CommonHttpRequestTest("PUT");
                Assert.Fail("Did not through an exception!");
            }
            catch (HttpException ex)
            {
                Assert.AreEqual(405, ex.GetHttpCode());
                Assert.AreEqual("The 'PUT' method is not allowed for this request", ex.Message);
            }
            catch (Exception)
            {
                Assert.Fail("Unexpected exception was thrown");
            }
        }
    }
}
