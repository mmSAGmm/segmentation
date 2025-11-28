using Microsoft.AspNetCore.SignalR;
using Segmentation.Domain.Abstractions;
using Segmentation.DomainModels;

namespace Segmentation.ApiService.Handlers
{
    public static class AdminHandlers
    {
        public static void MapAdminEndpoint(this WebApplication app)
        {
            app.MapGet("api/admin/v1/segment/{id:Guid}", GetSegment);
            app.MapGet("api/admin/v1/segment/init", Init);
            app.MapPost("api/admin/v1/segment", CreateSegment);
        }

        public static Task<Segment> GetSegment(Guid id, IBackofficeService service)
        {
            return service.Get(id);
        }

        public static Task Init(IBackofficeService service)
        {
            return service.Init();
        }

        public static async Task CreateSegment(Segment segment, IBackofficeService service) 
        {
            segment.Id = Guid.NewGuid();
            await service.Add(segment);
        }
    }
}
