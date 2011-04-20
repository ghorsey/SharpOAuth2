#region License
/* The MIT License
 * 
 * Copyright (c) 2011 Geoff Horsey
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using SharpOAuth2.Provider.Domain;
using SharpOAuth2.Provider.Framework;
namespace SharpOAuth2.Provider.Services
{
    /// <summary>
    /// Client service that or encapsulates that deals with the fetching and validating 
    /// clients <see cref="IClient" types/>
    /// </summary>
    public interface IClientService 
    {

        /// <summary>
        /// Authenticates the client.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// 	<c>true</c> if the client id and client secret match the values from the data store; otherwise <c>false</c>.
        /// </returns>
        bool AuthenticateClient(IOAuthContext context);
        /// <summary>
        /// Validates the redirect URI.
        /// </summary>
        /// <param name="context">The current authoricaton context<param>
        /// <returns><c>true</c> if the redirect uri matches the client record; otherwise, <c>false</c></returns>
        bool ValidateRedirectUri(IOAuthContext context);

        /// <summary>
        /// Determines whether the specified client is client.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// 	<c>true</c> if the specified client id matches an existing client; otherwise, <c>false</c>.
        /// </returns>
        bool IsClient(IOAuthContext context);

        /// <summary>
        /// Finds the client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        ClientBase FindClient(string clientId);

        /// <summary>
        /// Determines whether the resource owner has granted the client the specified scope.
        /// </summary>
        /// <param name="client">The client requesting access.</param>
        /// <param name="scope">The scope the client is requesting.</param>
        /// <param name="resourceOwnerId">The resource owner that the client is requesting access from.</param>
        /// <returns>
        /// 	<c>true</c> if the access is granted for the specified client; otherwise, <c>false</c>.
        /// </returns>
        bool IsAccessGranted(IClient client, string[] scope, string resourceOwnerId);
    }
}
