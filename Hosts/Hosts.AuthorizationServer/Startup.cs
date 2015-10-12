using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Hosts.AuthorizationServer.Stores;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
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

            var identityServerServiceFactory = new IdentityServerServiceFactory
            {
                ClientStore = new Registration<IClientStore>(typeof(ClientStore))
            }
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
            .UseInMemoryScopes(new List<Scope> {new Scope {Name = "Api"}});

            app.UseIdentityServer(new IdentityServerOptions
            {
                SiteName = "Skaele Authorization Server",
                Factory = identityServerServiceFactory,
                AuthenticationOptions = new AuthenticationOptions
                {
                    IdentityProviders = ConfigureIdentityProviders
                },
                SigningCertificate = GetCertificate("Hosts.AuthorizationServer.Certificates.SkaeleAuth.pfx", "secret")
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

        public static X509Certificate2 GetCertificate(string certificate, string password)
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