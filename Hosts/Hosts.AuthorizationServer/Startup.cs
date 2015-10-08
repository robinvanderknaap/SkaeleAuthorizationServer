using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Dependencies;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using Infrastructure.ClientProvider;
using Infrastructure.DependencyResolution;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Facebook;
using NLog;
using NLog.Config;
using NLog.Targets;
using Owin;
using StructureMap;
using Web.AuthorizationServer;

namespace Hosts.AuthorizationServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SetupLogger();

            var container = SetupContainer();

            
            

            var clients = container.GetInstance<IClientProvider>().GetClients();

            app.UseIdentityServer(new IdentityServerOptions
            {
                SiteName = "Skaele Authorization Server",
                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryUsers(new List<InMemoryUser>
                    {
                        new InMemoryUser
                        {
                            Username = "robink",
                            Password = "secret",
                            Subject = "robink",
                            Enabled = true
                        }
                    })
                    .UseInMemoryClients(new List<Client>{
                        new Client
                        {
                            ClientName = "ConsoleClient",
                            ClientId = "ConsoleClient",
                            AccessTokenType = AccessTokenType.Reference,
                            Flow = Flows.ClientCredentials, // No resource owner /user: machine->machine communication
                            AllowedScopes = new List<string> { "Api" }, // 'Scope' is the api (resource server) that we are protecting
                            ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) }
                        },
                        new Client
                        {
                            ClientName = "AngularClient",
                            ClientId = "AngularClient",
                            AccessTokenType = AccessTokenType.Reference,
                            Flow = Flows.ResourceOwner, // Machine->machine communication on behalf of user,
                            
                            AllowedScopes = new List<string> { "Api", StandardScopes.OfflineAccess.Name }, // 'Scope' is the api (resource server) that we are protecting
                            ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
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

                            AllowedScopes = new List<string> { "Api", StandardScopes.OfflineAccess.Name }, // 'Scope' is the api (resource server) that we are protecting
                            ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                            RefreshTokenUsage = TokenUsage.OneTimeOnly,
                            UpdateAccessTokenClaimsOnRefresh = true,
                            RefreshTokenExpiration = TokenExpiration.Sliding,
                            RedirectUris = new List<string> { "http://localhost:50929/Authenticate/external-login-callback" }
                        }

                    })
                    .UseInMemoryScopes(new List<Scope> { new Scope { Name = "Api" } }),
                    AuthenticationOptions = new AuthenticationOptions
                    {
                        IdentityProviders = ConfigureIdentityProviders
                    }

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

        private static void SetupLogger()
        {
            // IdentityServer uses LibLog, this means NLog is automatically detected.
            // We just need to setup NLog and it will receive logmessages from IdentityServer

            var loggingConfiguration = new LoggingConfiguration();

            // Setup debugger target which logs to debugger, log statements will be visible in output window of Visual Studio
            var debuggerTarget = new DebuggerTarget();
            loggingConfiguration.AddTarget("DebuggerTarget", debuggerTarget);
            loggingConfiguration.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, debuggerTarget));

            LogManager.Configuration = loggingConfiguration;

            LogManager.ThrowExceptions = true;
        }

        public static void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            var fb = new FacebookAuthenticationOptions
            {
                AuthenticationType = "Facebook",
                Caption = "Facebook",
                SignInAsAuthenticationType = signInAsType,
                AppId = "...",
                AppSecret = "..."
            };
            app.UseFacebookAuthentication(fb);
        }
    }

    
}