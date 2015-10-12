using System.Threading.Tasks;
using Hosts.AuthorizationServer.Certificates;
using Hosts.AuthorizationServer.Logging;
using Hosts.AuthorizationServer.Stores;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using Microsoft.Owin.Security.Facebook;
using Owin;

namespace Hosts.AuthorizationServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Logger.SetupLogger();

            app.UseIdentityServer(new IdentityServerOptions
            {
                SiteName = "Skaele Authorization Server",
                Factory = new IdentityServerServiceFactory
                {
                    ClientStore = new Registration<IClientStore>(typeof(ClientStore)),
                    ScopeStore = new Registration<IScopeStore>(typeof(ScopeStore)),
                    UserService = new Registration<IUserService>(typeof(UserService))
                },
                AuthenticationOptions = new AuthenticationOptions
                {
                    IdentityProviders = ConfigureIdentityProviders
                },
                SigningCertificate = Certificate.Get("Hosts.AuthorizationServer.Certificates.SkaeleAuth.pfx", "secret")
            });
        }

        

        private static void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
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

    public class UserService : IUserService
    {
        public Task PreAuthenticateAsync(PreAuthenticationContext context)
        {
            return Task.FromResult(0);
        }

        public Task AuthenticateLocalAsync(LocalAuthenticationContext context)
        {
            if (context.UserName == "robin@skaele.nl" && context.Password == "secret")
            {
                context.AuthenticateResult = new AuthenticateResult("robink", "Robin van der Knaap");
            }

            return Task.FromResult(0);
        }

        public Task AuthenticateExternalAsync(ExternalAuthenticationContext context)
        {
            context.AuthenticateResult = new AuthenticateResult("robink", "Robin van der Knaap");

            return Task.FromResult(0);
        }

        public Task PostAuthenticateAsync(PostAuthenticationContext context)
        {
            return Task.FromResult(0);
        }

        public Task SignOutAsync(SignOutContext context)
        {
            return Task.FromResult(0);
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
        }
    }
}