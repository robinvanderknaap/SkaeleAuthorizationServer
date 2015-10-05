using Infrastructure.Configuration;
using Infrastructure.Configuration.AppSettings;
using StructureMap.Configuration.DSL;

namespace Infrastructure.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<IConfiguration>().Use<AppSettingsConfiguration>();
        }
    }
}
