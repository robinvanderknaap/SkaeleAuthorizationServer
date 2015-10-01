using Infrastructure.ClientProvider;
using Infrastructure.ClientProvider.AppSettings;
using StructureMap.Configuration.DSL;

namespace Infrastructure.DependencyResolution
{
    public class IdentityServerRegistry : Registry
    {
        public IdentityServerRegistry()
        {
            For<IClientProvider>().Use<AppSettingsClientProvider>();
        }
    }
}
