using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.Domain;

namespace SharpOAuth2.Provider.Services
{
	public interface IAuthorizationGrantService : IAuthorizationGrant
	{
		IAuthorizationGrant FindAuthorizationGrant(string authorizationCode);
		IAuthorizationGrant IssueAuthorizationGrant(IAuthorizationContext context);
		void ConsumeGrant(IAuthorizationGrant grant);
		bool ValidateGrant(TokenEndpoint.ITokenContext context, IAuthorizationGrant grant);
	}
}