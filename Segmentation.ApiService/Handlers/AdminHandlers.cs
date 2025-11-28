using Microsoft.AspNetCore.SignalR;
using Segmentation.Domain.Abstractions;
using Segmentation.DomainModels;

namespace Segmentation.ApiService.Handlers
{
    public static class AdminHandlers
    {
        public static void MapAdminEndpoints(this WebApplication app)
        {
            app.MapDelete("api/admin/v1/segment/{id:Guid}", DeleteSegment);
            app.MapGet("api/admin/v1/segment/{id:Guid}", GetSegment);
            app.MapGet("api/admin/v1/segment/init", Init);
            app.MapPost("api/admin/v1/segment", CreateSegment);
            app.MapPut("api/admin/v1/segment", UpdateSegment);
            app.MapPut("api/admin/v1/segment/page/{number}/{size}", SegmentsPage);
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

        public static async Task UpdateSegment(Segment segment, IBackofficeService service)
        {
            await service.Update(segment);
        }

        public static async Task DeleteSegment(Guid id, IBackofficeService service)
        {
            await service.Delete(id);
        }

        public static async Task<IEnumerable<Segment>> SegmentsPage(
            int pageNumber, 
            int pageSize, 
            IBackofficeService service)
        {
            return await service.GetPage(pageNumber, pageSize);
        }
    }
}
