using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.TokenEndpoint;
using System.Web;
using Moq;

namespace SharpOAuth2.Tests.Provider.TokenEndpoint
{
    [TestFixture]
    public class TokenResponseWriterTests
    {
        [Test]
        public void TestWritingResponse()
        {
            TokenResponse response = new TokenResponse
            {
                Body = "body",
                HttpStatusCode = 200
            };

            Mock<HttpResponseBase> mckResponse = new Mock<HttpResponseBase>();
            mckResponse.SetupSet(x => x.ContentType = "application/json");
            mckResponse.SetupSet(x => x.StatusCode = 200);
            mckResponse.Setup(x => x.Write("body"));
            TokenResponseWriter writer = new TokenResponseWriter(mckResponse.Object);
            writer.WriteResponse(response);
            mckResponse.VerifyAll();
        }
    }
}
