using System;
using NUnit.Framework;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.TokenEndpoint;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Inspectors
{
    public abstract class InspectorTestBase
    {
        public void CommonAssertInspector(IContextInspector<ITokenContext> inspector, ITokenContext ctx)
        {
            try
            {
                inspector.Inspect(ctx);
                Assert.Fail("No Exception was thrown");
            }
            catch (OAuthErrorResponseException<ITokenContext> x)
            {
                Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.InvalidRequest, x.Error);
            }
            catch (Exception x)
            {
                Assert.Fail(x.Message);
            }

        }
    }
}
