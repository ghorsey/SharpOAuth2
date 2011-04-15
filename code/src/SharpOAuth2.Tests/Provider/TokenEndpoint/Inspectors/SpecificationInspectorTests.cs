using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.TokenEndpoint.Inspectors;
using SharpOAuth2.Provider;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Inspectors
{
    [TestFixture]
    public class SpecificationInspectorTests : InspectorTestBase
    {
        [Test]
        public void TestValidContext()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.AuthorizationCode;
            context.Client = new ClientBase
            {
                ClientId = "123",
                ClientSecret = "secret"
            };
            SpecificationInspector inspector = new SpecificationInspector();
            inspector.Inspect(context);
        }

        [Test, ExpectedException(typeof(OAuthFatalException))]
        public void TestNullClientContext()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.AuthorizationCode;
            SpecificationInspector inspector = new SpecificationInspector();
            inspector.Inspect(context);
        }

        [Test]
        public void TestContextMissingClientId()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.AuthorizationCode;
            context.Client = new ClientBase
            {
                ClientSecret = "secret"
            };
            SpecificationInspector inspector = new SpecificationInspector();
            CommonAssertInspector(inspector, context);
        }

        [Test]
        public void TestContextMissingClientSecret()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.AuthorizationCode;
            context.Client = new ClientBase
            {
                ClientId = "123"
            };
            SpecificationInspector inspector = new SpecificationInspector();
            CommonAssertInspector(inspector, context);
        }

        [Test]
        public void TestInvalidValidContext()
        {
            TokenContext context = new TokenContext();

            context.GrantType = "";

            SpecificationInspector inspector = new SpecificationInspector();
            try
            {
                inspector.Inspect(context);
                Assert.Fail("Did not throw an exception");
            }
            catch (OAuthErrorResponseException<ITokenContext> x)
            {
                Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.InvalidRequest, x.Error);
            }
            catch (Exception)
            {
                Assert.Fail("Unexpected exception was thrown");
            }
        }
    }
}
