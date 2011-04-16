using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpOAuth2.Provider.TokenEndpoint;
using SharpOAuth2.Fluent;
namespace SharpOAuth2.Mvc
{
    public class TokenResponseActionResult : System.Web.Mvc.ActionResult
    {
        readonly TokenResponse TokenResponse;

        public TokenResponseActionResult(TokenResponse tokenResponse)
        {
            TokenResponse = tokenResponse;
        }
        public override void ExecuteResult(System.Web.Mvc.ControllerContext context)
        {
            context.HttpContext.Response.WriteTokenResponse(TokenResponse);
        }
    }
}
