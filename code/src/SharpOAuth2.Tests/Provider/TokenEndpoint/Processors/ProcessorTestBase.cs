using System;
using NUnit.Framework;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Provider.TokenEndpoint.Processor;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint.Processors
{
    public class ProcessorTestBase
    {
        protected void CommonProcessorErrorAssert(AuthenticationCodeProcessor processor, ITokenContext context, string errorParamter)
        {
            try
            {
                processor.Process(context);
                Assert.Fail("Did not produce an exception");
            }
            catch (OAuthErrorResponseException<IOAuthContext> x)
            {
                Assert.AreEqual(errorParamter, x.Error);
            }
            catch (OAuthErrorResponseException<ITokenContext> x)
            {
                Assert.AreEqual(errorParamter, x.Error);
            }
            catch (Exception x)
            {
                Assert.Fail("Unexpected exception was thrown: " + x.Message);
            }
        }
    }
}
