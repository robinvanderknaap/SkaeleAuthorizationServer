using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;

namespace Hosts.AuthorizationServer.Stores
{
    public class ScopeStore : IScopeStore
    {
        private readonly IEnumerable<Scope> _scopes = new List<Scope>
        {
            new Scope
            {
                Name = "Api"
            }
        }; 

        public Task<IEnumerable<Scope>> FindScopesAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(_scopes);
        }

        public Task<IEnumerable<Scope>> GetScopesAsync(bool publicOnly = true)
        {
            return Task.FromResult(_scopes);
        }
    }
}