using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DataAccess.Implementation;
using Segmentation.DataAccess.Options;
using Segmentation.Domain.Abstractions;
using Segmentation.Domain.Implementation;

namespace Segmentation.Domain
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBackofficeService, BackofficeService>();
            services.AddSingleton<IPropertiesService, PropertiesService>();
            return services;
        }
    }
}
