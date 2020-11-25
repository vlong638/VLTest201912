using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace VL.Consoling.Configuration
{
    public class ConfigurationHelper
    {
        public static IConfigurationRoot Build(string filename)
        {
            return new ConfigurationBuilder().AddJsonFile(filename).Build();
        }
    }
}
