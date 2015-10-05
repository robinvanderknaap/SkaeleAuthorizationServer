using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.InMemory;
using Infrastructure.ClientProvider;
using Infrastructure.DependencyResolution;
using Microsoft.Owin.Cors;
using Owin;
using StructureMap;
using Web.AuthorizationServer;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

namespace Hosts.AuthorizationServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Create container
            var container = SetupContainer();

            var clients = container.GetInstance<IClientProvider>().GetClients();

            app.UseIdentityServer(new IdentityServerOptions
            {
                SiteName = "Skaele Authorization Server",
                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryUsers(new List<InMemoryUser>
                    {
                        
                    })
                    .UseInMemoryClients(new List<Client>{
    
                    })
                    .UseInMemoryScopes(StandardScopes.All)
                    
            });

            // Configure CORS (Cross Origin Resource Sharing)
            app.UseCors(CorsOptions.AllowAll);

            // Create new http configuration
            var config = new HttpConfiguration();

            // Configure webapi
            WebApiConfig.Configure(config, container.GetInstance<IDependencyResolver>());

            // Register web api
            app.UseWebApi(config);
        }

        private Container SetupContainer()
        {
            return new Container(config =>
            {
                config.AddRegistry<WebApiRegistry>();
                config.AddRegistry<ConfigurationRegistry>();
                config.AddRegistry<IdentityServerRegistry>();
            });
        }
    }

    
}