using Common.DataAccess.Abstraction;
using Common.DataAccess.Implementation;
using Common.DataAccess.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DataAccess
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddCommonDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionProvider, SqliteConnectionProvider>();
            services.Configure<SQLiteOption>(configuration.GetSection("SQLite"));
            services.Configure<QueryOption>(configuration.GetSection("Query"));
            return services;
        }
    }
}
