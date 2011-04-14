using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.AuthorizationEndpoint.Inspectors;
using SharpOAuth2.Provider;

namespace SharpOAuth2.Tests.Provider.AuthorizationEndpoint.Inspectors
{
    [TestFixture]
    public class SpecificationInspectorTests
    {
        private static IAuthorizationContext MakeAuthorizationContext()
        {
            IAuthorizationContext context = new AuthorizationContext();
            context.Client = new ClientBase { ClientId = "12345", ClientSecret = "secret" };
            context.RedirectUri = new Uri("http://www.mysite.com", UriKind.RelativeOrAbsolute);
            context.ResponseType = Parameters.ResponseTypeValues.AuthorizationCode;
            context.Scope = new string[] { "create-customer", "view-clients" };
            context.State = "my state";
            return context;
        }

        private static void CommonInvalidRequestSpecificationTest(IAuthorizationContext context, string parameter)
        {
            IAuthorizationContextInspector inspector = new SpecificationInspector();
            try
            {
                inspector.Insepct(context);
                Assert.Fail("No exception was thrown");
            }
            catch (OAuthErrorResponseException<IAuthorizationContext> ex)
            {
                Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.InvalidRequest, ex.Error);
                Assert.AreEqual("The '" + parameter + "' parameter is required and missing, unsupported, or malformed", ex.Message);
            }
            catch (Exception)
            {
                Assert.Fail("Threw an unexpected exception");
            }
        }
        [Test]
        public void TestResponseTypeIsRequired()
        {
            IAuthorizationContext context = MakeAuthorizationContext();

            context.ResponseType = null;

            CommonInvalidRequestSpecificationTest(context, Parameters.ResponseType);
            
        }

        [Test]
        public void TestClientIdIsRequired()
        {
            IAuthorizationContext context = MakeAuthorizationContext();
            context.Client.ClientId = null;

            CommonInvalidRequestSpecificationTest(context, Parameters.ClientId);

            context.Client = null;

            CommonInvalidRequestSpecificationTest(context, Parameters.ClientId);
        }

        [Test]
        public void TestRedirectUriIsRequired()
        {
            IAuthorizationContext context = MakeAuthorizationContext();

            context.RedirectUri = null;

            CommonInvalidRequestSpecificationTest(context, Parameters.RedirectUri);

        }
        [Test]
        public void TestInvalidResponseTypeValue()
        {
            IAuthorizationContext context = MakeAuthorizationContext();

            context.ResponseType = "unk";

            IAuthorizationContextInspector inspector = new SpecificationInspector();

            try
            {
                inspector.Insepct(context);
                Assert.Fail("Did not throw exception");
            }
            catch (OAuthErrorResponseException<IAuthorizationContext> ex)
            {
                Assert.AreEqual(Parameters.ErrorParameters.ErrorValues.UnsupportedResponseType, ex.Error);
                Assert.AreEqual("The 'unk' response type is unsupported", ex.Message);
            }
            catch (Exception)
            {
                Assert.Fail("Threw an unexpected excetpion");
            }
        }

        [Test]
        public void TestSuccessfulSpecificationInspector()
        {
            IAuthorizationContext context = MakeAuthorizationContext();
            IAuthorizationContextInspector inspector = new SpecificationInspector();

            inspector.Insepct(context);
        }
        
    }
}
