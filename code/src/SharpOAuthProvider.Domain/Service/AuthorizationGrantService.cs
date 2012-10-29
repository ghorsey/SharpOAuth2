using System;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Services;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuth2.Provider.AuthorizationEndpoint;

namespace SharpOAuthProvider.Domain.Service
{
	public class AuthorizationGrantService : AuthorizationGrantBase, IAuthorizationGrantService
	{
		readonly IClientRepository ClientRepo;
		readonly ITokenRepository TokenRepo;

		public AuthorizationGrantService(ITokenRepository tokenRepo, IClientRepository clientRepo)
		{
			TokenRepo = tokenRepo;
			ClientRepo = clientRepo;
		}

		#region IAuthorizationGrantService Members

		public IAuthorizationGrant FindAuthorizationGrant(string authorizationCode)
		{
			return TokenRepo.FindAuthorizationGrant(authorizationCode);
		}

		public IAuthorizationGrant IssueAuthorizationGrant(IAuthorizationContext context)
		{
			AuthorizationGrant grant = new AuthorizationGrant();
			Client client = ClientRepo.FindClient(context.Client.ClientId);
			grant.Client = client;
			grant.ResourceOwnerId = context.ResourceOwnerUsername;
			//grant.IsApproved = false;
			//grant.ExpiresIn = 120; // 2 minutes
			grant.Code = Guid.NewGuid().ToString();
			grant.IssuedOn = SharpOAuth2.Provider.Utility.Epoch.ToEpoch(DateTime.Now);
			grant.Scope = string.Join(" ", context.Scope);

			TokenRepo.AddAuthorizationGrant(grant);
			return grant;
		}

		public void ConsumeGrant(IAuthorizationGrant grant)
		{
			AuthorizationGrant gr = (AuthorizationGrant)grant;
			gr.IsUsed = true;
			TokenRepo.AddAuthorizationGrant(gr);
		}

		public bool ValidateGrant(SharpOAuth2.Provider.TokenEndpoint.ITokenContext context, IAuthorizationGrant grant)
		{
			return true;
		}

		#endregion
	}
}