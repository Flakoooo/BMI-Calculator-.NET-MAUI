using Microsoft.Extensions.Configuration;
using Supabase;
using System.Reflection;

namespace BodyMassIndexCalculator.src.Services
{
    public static class SupabaseService
    {
        private static readonly string _configFileName = "appsettings.json";
        private static readonly Lazy<(string Url, string Key)> _config = new(GetSupabaseConfig());
        private static readonly Lazy<Client> _client = new(
            () => new Client(_config.Value.Url, _config.Value.Key)
        );

        public static Client Client => _client.Value;

        private static Stream LoadConfigFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetManifestResourceStream($"BodyMassIndexCalculator.{_configFileName}") ?? Stream.Null;
        }

        private static (string Url, string Key) GetSupabaseConfig()
        {
            using var stream = LoadConfigFile();
            if (stream == Stream.Null)
                throw new FileNotFoundException("Конфигурационный файл не найден");

            var config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            return (
                config["Supabase:Url"] ?? throw new ArgumentNullException("Supabase URL не найден"),
                config["Supabase:AnonKey"] ?? throw new ArgumentNullException("Supabase AnonKey не найден")
            );
        }

        public static async Task Initialize() => await Client.InitializeAsync();
    }
}
