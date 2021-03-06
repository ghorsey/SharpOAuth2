﻿using System;
using SharpOAuth2.Framework;
using SharpOAuth2.Provider.AuthorizationEndpoint;
using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
using SharpOAuth2.Provider.Services;
using SharpOAuthProvider.Domain.Repository;
using SharpOAuth2.Provider.TokenEndpoint;

namespace SharpOAuthProvider.Domain.Service
{
	public class TokenService : ITokenService
	{
		readonly IClientRepository ClientRepo;
		readonly ITokenRepository TokenRepo;

		public TokenService(IClientRepository clientRepo, ITokenRepository tokenRepo)
		{
			ClientRepo = clientRepo;
			TokenRepo = tokenRepo;
		}

		#region ITokenService Members



		public IToken IssueAccessToken(IAuthorizationGrant grant)
		{
			//typically here you'd fetch your access
			AccessToken token = new AccessToken
			{
				ExpiresIn = 120,
				Token = Guid.NewGuid().ToString(),
				Grant = (AuthorizationGrant)grant
			};
			token.Scope = ((AuthorizationGrant)grant).Scope.Split(' ');

			TokenRepo.AddAccessToken(token);
			return token;

		}

		public IAccessToken FindToken(string token)
		{
			return TokenRepo.FindToken(token);
		}

		public IToken IssueAccessTokenForResourceOwner(ITokenContext context)
		{

			AccessToken token = new AccessToken
			{
				ExpiresIn = 120,
				Token = Guid.NewGuid().ToString(),
				RefreshToken = Guid.NewGuid().ToString(),
				Scope = new string[] { "create", "delete", "view" },
			};
			TokenRepo.AddAccessToken(token);
			return token;

		}

		public IToken IssueAccessToken(ClientBase client)
		{

			AccessToken token = new AccessToken
			{
				ExpiresIn = 120,
				Token = Guid.NewGuid().ToString(),
				Scope = new string[] { "create-member" },
				Client = (Client)client
			};

			TokenRepo.AddAccessToken(token);

			return token;
		}

		public IToken IssueAccessToken(RefreshTokenBase refreshToken)
		{
			AccessToken token = new AccessToken
			{
				ExpiresIn = 120,
				Token = Guid.NewGuid().ToString(),
				RefreshToken = refreshToken.Token,
				Scope = refreshToken.Scope,
				TokenType = "bearer"
			};
			TokenRepo.AddAccessToken(token);
			return token;
		}

		public RefreshTokenBase FindRefreshToken(string refreshToken)
		{
			return TokenRepo.FindRefreshToken(refreshToken);
		}

		#endregion

	}
}
