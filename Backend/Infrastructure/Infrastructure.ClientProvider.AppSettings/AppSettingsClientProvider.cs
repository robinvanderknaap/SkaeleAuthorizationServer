using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer3.Core.Models;
using Infrastructure.Configuration;

namespace Infrastructure.ClientProvider.AppSettings
{
    public class AppSettingsClientProvider : IClientProvider
    {
        private readonly IConfiguration _configuration;

        public AppSettingsClientProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IList<Client> GetClients()
        {
            return _configuration.Get<List<ConfigClient>>("identityserver:clients").Select(x => new Client
            {
                Enabled = x.Enabled,
                ClientName = x.ClientName,
                ClientId = x.ClientId,
                Flow = x.Flow,
                RedirectUris = x.RedirectUrls,
                AllowedScopes = x.Scopes
            }).ToList();
        }
    }
}
