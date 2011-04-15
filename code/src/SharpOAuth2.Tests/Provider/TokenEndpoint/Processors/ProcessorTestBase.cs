using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.TokenEndpoint.Processor;
using SharpOAuth2.Provider.TokenEndpoint;
using NUnit.Framework;
using SharpOAuth2.Provider;

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
