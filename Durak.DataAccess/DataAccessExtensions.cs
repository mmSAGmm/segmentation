using Durak.DataAccess.Abtractions;
using Durak.DataAccess.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Durak.DataAccess.DataAccess
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddDurakDataAccess(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddSingleton<IGameRepository, GameRepository>();
            return services;
        }
    }
}
