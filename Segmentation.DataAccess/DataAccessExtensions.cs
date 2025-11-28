using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DataAccess.Automapper;
using Segmentation.DataAccess.Implementation;
using Segmentation.DataAccess.Options;

namespace Segmentation.DataAccess
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddSingleton<IConnectionProvider, SqliteConnectionProvider>();
            services.AddSingleton<ISegmentsRepository, SegmentsRepository>();
            services.AddAutoMapper(x => x.AddProfile<DbMappings>());
            services.Configure<SQLiteOption>(configuration.GetSection("SQLite"));
            return services;
        }
    }
}
