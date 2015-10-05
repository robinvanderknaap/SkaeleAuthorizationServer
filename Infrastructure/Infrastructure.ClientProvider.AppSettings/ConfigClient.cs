using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace Infrastructure.ClientProvider.AppSettings
{
    public class ConfigClient
    {
        public bool Enabled { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]
        public Flows Flow { get; set; }
        public List<string> RedirectUrls { get; set; }
        public List<string> Scopes { get; set; }
    }
}