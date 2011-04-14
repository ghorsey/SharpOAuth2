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
    public class SpecificationInspectorTests
    {
        [Test]
        public void TestValidContext()
        {
            TokenContext context = new TokenContext();

            context.GrantType = Parameters.GrantTypeValues.AuthorizationCode;

            SpecificationInspector inspector = new SpecificationInspector();
            inspector.Inspect(context);
        }

        [Test]
        public void TestInvalidValidContext()
        {
            TokenContext context = new TokenContext();

            context.GrantType = "Unknown";

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
