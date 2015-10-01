using System.Web.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Web.Api
{
    public class WebApiConfig
    {
        public static void Configure(
            HttpConfiguration config,
            System.Web.Http.Dependencies.IDependencyResolver dependencyResolver
        )
        {
            // Set dependency resolver
            config.DependencyResolver = dependencyResolver;

            // Setup attribute routing
            config.MapHttpAttributeRoutes();

            // Setup json formatter to use camelcasing even when responses are pascal cased
            // http://msmvps.com/blogs/theproblemsolver/archive/2014/03/26/webapi-pascalcase-and-camelcase.aspx
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Converting enums to string instead of ints
            // http://stackoverflow.com/questions/20242035/mediatypeformatter-serialize-enum-string-values-in-web-api
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            // Remove xml formatter, we only support json.
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Setup unhandled exception logger
            //config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());
            //config.Services.Replace(typeof(IExceptionHandler), new UnhandledExceptionHandler());
        }
    }
}
