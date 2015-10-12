using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;
using Microsoft.Owin.Security.Facebook;
using NLog;
using NLog.Config;
using NLog.Targets;
using Owin;

namespace Hosts.AuthorizationServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SetupLogger();

            app.UseIdentityServer(new IdentityServerOptions
            {
                SiteName = "Skaele Authorization Server",
                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryUsers(new List<InMemoryUser>
                    {
                        new InMemoryUser
                        {
                            Username = "robin@skaele.nl",
                            Password = "secret",
                            Subject = "robink",
                            Enabled = true,
                            Claims = new List<Claim>
                            {
                                new Claim("Role", "admin"),
                                new Claim("Fullname", "Robin van der Knaap")
                            }
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
                            AccessTokenType = AccessTokenType.Jwt,
                            Flow = Flows.ResourceOwner, // Machine->machine communication on behalf of user,
                            
                            AllowedScopes = new List<string> { "Api" }, // 'Scope' is the api (resource server) that we are protecting
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
                    },
                    SigningCertificate = Get("Hosts.AuthorizationServer.Certificates.SkaeleAuth.pfx", "secret")

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

        public static X509Certificate2 Get(string certificate, string password)
        {
            var assembly = typeof(Startup).Assembly;
            using (var stream = assembly.GetManifestResourceStream(certificate))
            {
                return new X509Certificate2(ReadStream(stream), password);
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }

    
}