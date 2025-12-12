using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DataAccess.Automapper;
using Segmentation.DataAccess.Implementation;

namespace Segmentation.DataAccess
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection AddSegmentationDataAccess(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddSingleton<ISegmentsRepository, SegmentsRepository>();
            services.AddSingleton<IPropertiesRepository, PropertiesRepository>();

            services.AddAutoMapper(x => x.AddProfile<DbMappings>());
            return services;
        }
    }
}
