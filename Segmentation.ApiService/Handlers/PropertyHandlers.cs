using Segmentation.Domain.Abstractions;
using Segmentation.DomainModels;

namespace Segmentation.ApiService.Handlers
{
    public static class PropertyHandlers
    {
        public static void MapPropertyEndpoints(this WebApplication app)
        {
            app.MapPost("api/business/v1/properties/{id}", Save);
            app.MapGet("api/business/v1/properties/{id}", Get);
            app.MapGet("api/business/v1/properties/init", Init);
        }

        public static Task<Dictionary<string, object>> Get(string id, IPropertiesService service)
        {
            return service.Get(id);
        }

        public static Task Init(IPropertiesService service)
        {
            return service.Init();
        }

        public static async Task Save(string id, Dictionary<string, object> properties, IPropertiesService service)
        {
            await service.Set(properties, id);
        }
    }
}
