using Microsoft.Extensions.Configuration;
using Quze.Infrastruture.Extensions;
using System.IO;

namespace Quze.Infrastruture.Utilities
{
    public class AppConfiguration
    {
        private readonly string _connectionString = string.Empty;
        private readonly IConfigurationSection appSettings;
        private static IConfigurationSection staticAppSettings;
        public AppConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            _connectionString = root.GetSection("ConnectionString").GetSection("DataConnection").Value;
            appSettings = root.GetSection("ApplicationSettings");
        }
        public string ConnectionString
        {
            get => _connectionString;
        }

        //public IConfigurationSection ApplicationSettings { get => appSettings; }

        public string this[string key]
        {
            get => appSettings.GetSection(key).Value;
        }


        public static string AppSettings(string key)
        {
            if (staticAppSettings.IsNull())
            {
                var configurationBuilder = new ConfigurationBuilder();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                configurationBuilder.AddJsonFile(path, false);

                var root = configurationBuilder.Build();
                staticAppSettings = root.GetSection("ApplicationSettings");
            }
            return staticAppSettings.GetSection(key).Value;
        }

    }
}
