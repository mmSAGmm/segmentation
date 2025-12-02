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
            app.MapGet("api/admin/v1/segment/page/{number}/{size}", SegmentsPage);
        }

        public static Task<Segment> GetSegment(Guid id, ISegmentAdminService service, CancellationToken token)
        {
            return service.Get(id, token);
        }

        public static Task Init(ISegmentAdminService service)
        {
            return service.Init();
        }

        public static async Task CreateSegment(Segment segment, ISegmentAdminService service, CancellationToken token)
        {
            segment.Id = Guid.NewGuid();
            await service.Add(segment, token);
        }

        public static async Task UpdateSegment(Segment segment, ISegmentAdminService service, CancellationToken token)
        {
            await service.Update(segment, token);
        }

        public static async Task DeleteSegment(Guid id, ISegmentAdminService service, CancellationToken token)
        {
            await service.Delete(id, token);
        }

        public static async Task<IEnumerable<Segment>> SegmentsPage(
            int number, 
            int size, 
            ISegmentAdminService service, CancellationToken token)
        {
            return await service.GetPage(number, size, token);
        }
    }
}
