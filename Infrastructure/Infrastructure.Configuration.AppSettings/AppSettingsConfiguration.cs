using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace Infrastructure.Configuration.AppSettings
{
    public class AppSettingsConfiguration : IConfiguration
    {
        [DebuggerStepThrough]
        public TValue Get<TValue>(string key)
        {
            // Make sure setting exists
            if (!ConfigurationManager.AppSettings.AllKeys.Contains(key))
            {
                throw new ApplicationException($"Application setting '{key}' not found, please update configuration file.");
            }

            // Retrieve setting value
            var value = ConfigurationManager.AppSettings[key];

            if (typeof(TValue).IsClass)
            {
                return JsonConvert.DeserializeObject<TValue>(value);
            }

            // Convert value to specified type and return
            return (TValue)Convert.ChangeType(value, typeof(TValue), CultureInfo.InvariantCulture);
        }

        public TValue Get<TValue>(Func<IConfiguration, TValue> config)
        {
            return config(this);
        }
    }
}
