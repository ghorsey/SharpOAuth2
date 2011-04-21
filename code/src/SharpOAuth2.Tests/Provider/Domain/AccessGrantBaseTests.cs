using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.Domain;

namespace SharpOAuth2.Tests.Provider.Domain
{
    [TestFixture]
    public class AccessGrantBaseTests
    {
        [Test, ExpectedException(typeof(NotSupportedException))]
        public void TestParametersPropertyNotSupported()
        {
            AuthorizationGrantBase grant = new AuthorizationGrantBase();
            int count = grant.Parameters.Count;
        }

    }
}
