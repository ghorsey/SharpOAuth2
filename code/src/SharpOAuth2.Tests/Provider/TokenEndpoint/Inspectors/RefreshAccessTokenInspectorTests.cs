using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.TokenEndpoint.Inspectors;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Inspectors
{
    [TestFixture]
    public class RefreshAccessTokenInspectorTests : InspectorTestBase
    {
        [Test]
        public void TestValidContext()
        {
            TokenContext context = new TokenContext
            {
                GrantType = Parameters.GrantTypeValues.RefreshToken,
                RefreshToken = "refresh",
                Scope = new string[] { "create", "delete" }
            };

            RefreshAccessTokenInspector inspector = new RefreshAccessTokenInspector();

            inspector.Inspect(context);
        }

        [Test]
        public void TestWrongGrantType()
        {
            TokenContext context = new TokenContext
            {
                GrantType = Parameters.GrantTypeValues.ClientCredentials,
            };

            RefreshAccessTokenInspector inspector = new RefreshAccessTokenInspector();

            inspector.Inspect(context);
        }


        [Test]
        public void TestMissingRefreshTokenContext()
        {
            TokenContext context = new TokenContext
            {
                GrantType = Parameters.GrantTypeValues.RefreshToken,
                Scope = new string[] { "create", "delete" }
            };

            RefreshAccessTokenInspector inspector = new RefreshAccessTokenInspector();

            CommonAssertInspector(inspector, context);
        }
    }
}
