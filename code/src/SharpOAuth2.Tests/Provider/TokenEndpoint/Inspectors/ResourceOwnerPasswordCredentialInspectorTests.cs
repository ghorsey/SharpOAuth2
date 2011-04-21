using NUnit.Framework;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.TokenEndpoint.Inspectors;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Inspectors
{
    [TestFixture]
    public class ResourceOwnerPasswordCredentialInspectorTests : InspectorTestBase
    {
        [Test]
        public void TestValidContext()
        {
            TokenContext ctx = new TokenContext
            {
                GrantType = Parameters.GrantTypeValues.Password,
                ResourceOwnerUsername = "username",
                ResourceOwnerPassword = "password",
                Scope = new string[] { "create", "delete" }
            };

            ResourceOwnerPasswordCredentialInspector inspector = new ResourceOwnerPasswordCredentialInspector();

            inspector.Inspect(ctx);
        }

        [Test]
        public void TestWronGrantType()
        {
            TokenContext ctx = new TokenContext
            {
                GrantType = Parameters.GrantTypeValues.AuthorizationCode
            };

            ResourceOwnerPasswordCredentialInspector inspector = new ResourceOwnerPasswordCredentialInspector();

            inspector.Inspect(ctx);
        }
        [Test]
        public void TestContextMissingUsername()
        {
            TokenContext ctx = new TokenContext
            {
                GrantType = Parameters.GrantTypeValues.Password,
                
                ResourceOwnerPassword = "password",
                Scope = new string[] { "create", "delete" }
            };

            ResourceOwnerPasswordCredentialInspector inspector = new ResourceOwnerPasswordCredentialInspector();

            CommonAssertInspector(inspector, ctx);
        }

        [Test]
        public void TestContextMissingPassword()
        {
            TokenContext ctx = new TokenContext
            {
                GrantType = Parameters.GrantTypeValues.Password,
                ResourceOwnerUsername = "username",
                Scope = new string[] { "create", "delete" }
            };

            ResourceOwnerPasswordCredentialInspector inspector = new ResourceOwnerPasswordCredentialInspector();

            CommonAssertInspector(inspector, ctx);
        }
    }
}
