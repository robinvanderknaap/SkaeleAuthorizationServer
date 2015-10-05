using System.Web.Http.Dependencies;
using Infrastructure.WebApiDependencyResolver.StructureMap;
using StructureMap.Configuration.DSL;

namespace Infrastructure.DependencyResolution
{
    public class WebApiRegistry : Registry
    {
        public WebApiRegistry()
        {
            For<IDependencyResolver>().Use<StructureMapWebApiResolver>();
        }
    }
}
