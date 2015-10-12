using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

namespace Hosts.AuthorizationServer.Stores
{
    public class ClientStore : IClientStore
    {
        private readonly IList<Client> _clients = new List<Client>
        {
            new Client
            {
                ClientName = "ConsoleClient",
                ClientId = "ConsoleClient",
                AccessTokenType = AccessTokenType.Reference,
                Flow = Flows.ClientCredentials, // No resource owner /user: machine->machine communication
                AllowedScopes = new List<string> {"Api"}, // 'Scope' is the api (resource server) that we are protecting
                ClientSecrets = new List<Secret> {new Secret("secret".Sha256())}
            },
            new Client
            {
                ClientName = "AngularClient",
                ClientId = "AngularClient",
                AccessTokenType = AccessTokenType.Jwt,
                Flow = Flows.ResourceOwner, // Machine->machine communication on behalf of user,
                AllowedScopes = new List<string> {"Api"}, // 'Scope' is the api (resource server) that we are protecting
                ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenExpiration = TokenExpiration.Sliding
            },
            new Client
            {
                ClientName = "AngularClient2",
                ClientId = "AngularClient2",
                AccessTokenType = AccessTokenType.Reference,
                Flow = Flows.AuthorizationCode, // User authorizes app to access api on his behalf
                AllowedScopes = new List<string> {"Api", StandardScopes.OfflineAccess.Name},
                // 'Scope' is the api (resource server) that we are protecting
                ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                RedirectUris = new List<string> {"http://localhost:50929/Authenticate/external-login-callback"}
            }

        };

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            return Task.FromResult(_clients.SingleOrDefault(x => x.ClientId == clientId));
        }
    }
}