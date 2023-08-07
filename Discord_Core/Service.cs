using Discord_Core.Database;
using Discord_Core.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Discord_Core
{
    public static class Service
    {
        public static IServiceCollection serviceCollection(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<DatabaseContext>();

            return serviceCollection;
        }

        //
        public static string GetConnectionString()
        {
            string appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            string json = File.ReadAllText(appSettingsPath);

            var data = JsonConvert.DeserializeObject<AppSettings>(json);

            var connectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = $"{data.SageDatabase.Host},{data.SageDatabase.Port}",
                InitialCatalog = $"{data.SageDatabase.Database}",
                Password = $"{data.SageDatabase.Password}",
                UserID = $"{data.SageDatabase.User}",
                TrustServerCertificate = true
            };
            return connectionStringBuilder.ToString();
        }
    }
}