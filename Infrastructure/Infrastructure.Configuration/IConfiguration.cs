using System;

namespace Infrastructure.Configuration
{
    public interface IConfiguration
    {
        TValue Get<TValue>(string key);
        TValue Get<TValue>(Func<IConfiguration, TValue> config);
    }
}
