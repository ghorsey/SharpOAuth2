using System;
using NUnit.Framework;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.TokenEndpoint.Inspectors;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Inspectors
{
    [TestFixture]
    public class AuthorizationCodeInspectorTest : InspectorTestBase
    {

        [Test]
        public void TestValidContext()
        {
            TokenContext ctx = new TokenContext()
            {
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                AuthorizationCode = "auth_code",
                Client = new ClientBase
                {
                    ClientId= "123",
                    ClientSecret = "secret"
                },
                RedirectUri = new Uri("http://www.mysite.com/callback")
            };

            AuthorizationCodeInspector inspector = new AuthorizationCodeInspector();
            inspector.Inspect(ctx);

        }

        [Test]
        public void TestWrongGrantType()
        {
            TokenContext ctx = new TokenContext()
            {
                GrantType = Parameters.GrantTypeValues.ClientCredentials,
            };

            AuthorizationCodeInspector inspector = new AuthorizationCodeInspector();
            inspector.Inspect(ctx);

        }

        [Test]
        public void TestMissingAuthCode()
        {
            TokenContext ctx = new TokenContext()
            {
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                Client = new ClientBase
                {
                    ClientId = "123",
                    ClientSecret = "secret"
                },
                RedirectUri = new Uri("http://www.mysite.com/callback")
            };

            AuthorizationCodeInspector inspector = new AuthorizationCodeInspector();

            CommonAssertInspector(inspector, ctx);

        }

        [Test]
        public void TestMissingRedirectUri()
        {
            TokenContext ctx = new TokenContext()
            {
                GrantType = Parameters.GrantTypeValues.AuthorizationCode,
                AuthorizationCode = "auth_code",
                Client = new ClientBase
                {
                    ClientId = "123",
                    ClientSecret = "secret"
                }
            };

            AuthorizationCodeInspector inspector = new AuthorizationCodeInspector();

            CommonAssertInspector(inspector, ctx);

        }

    }
}
