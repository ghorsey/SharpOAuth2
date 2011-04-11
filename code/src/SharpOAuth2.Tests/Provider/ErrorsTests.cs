using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider;
using SharpOAuth2.Provider.Authorization;

namespace SharpOAuth2.Tests.Provider
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
