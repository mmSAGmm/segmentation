using Durak.Engine.Domain.Abtractions;
using Durak.Engine.Domain.Implementation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Durak.Engine.Domain
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDurakDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IGameService, GameService>();

            return services;
        }
    }
}
