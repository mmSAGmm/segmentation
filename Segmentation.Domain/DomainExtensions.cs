using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DataAccess.Implementation;
using Segmentation.DataAccess.Options;
using Segmentation.Domain.Abstractions;
using Segmentation.Domain.Implementation;
using Segmentation.Domain.Options;

namespace Segmentation.Domain
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISegmentAdminService, SegmentAdminService>();
            services.AddSingleton<IPropertiesService, PropertiesService>();
            services.AddSingleton<IExpressionCompilationService, ExpressionCompilationService>();
            services.AddSingleton<IEvaluationService, EvaluationService>();
            services.AddSingleton<IExpressionCache, ExpressionCache>();
            services.Configure<ExpressionCacheOptions>(configuration.GetSection("cache:ExpressionCache"));
            return services;
        }
    }
}
