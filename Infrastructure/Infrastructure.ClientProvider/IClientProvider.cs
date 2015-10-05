using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace Infrastructure.ClientProvider
{
    public interface IClientProvider
    {
        IList<Client> GetClients();
    }
}
