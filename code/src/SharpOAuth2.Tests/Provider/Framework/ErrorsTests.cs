using System;
using NUnit.Framework;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.Framework;

namespace SharpOAuth2.Tests.Provider.Framework
{
    [TestFixture]
    public class ErrorsTests
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestInvalidRequestMissingParameterException()
        {
            Errors.InvalidRequestException(new AuthorizationContext(), null);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestUnsupportedResponseTypeMissingParameterException()
        {
            Errors.UnsupportedResponseType(new AuthorizationContext(), null);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestInvalidRequestNullContextException()
        {
            Errors.InvalidRequestException((IAuthorizationContext)null, "my param");
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void TestUnsupportedResponseTypeNullContextException()
        {

            Errors.UnsupportedResponseType((IAuthorizationContext)null, "my param");
        }
    }
}
