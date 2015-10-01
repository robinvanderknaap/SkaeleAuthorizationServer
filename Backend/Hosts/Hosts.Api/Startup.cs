using System.Web.Http;
using System.Web.Http.Dependencies;
using Infrastructure.DependencyResolution;
using Microsoft.Owin.Cors;
using Owin;
using StructureMap;
using Web.Api;


namespace Hosts.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Create container
            var container = SetupContainer();

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
            });
        }
    }
}