using Segmentation.ApiService.ResponseModel;
using Segmentation.Domain.Abstractions;
using System.Runtime.Intrinsics.Arm;

namespace Segmentation.ApiService.Handlers
{
    public static class EvaluateHandlers
    {
        public static void MapEvaluateEndpoints(this WebApplication app)
        {
            app.MapPost("api/business/v1/evaluate/{segmentId:Guid}/{propertiesId}", Evaluate);
        }

        public static async Task<EvaluateResponseModel> Evaluate(Guid segmentId, string propertiesId, IEvaluationService service)
        {
            var result = await service.Evaluate(segmentId, propertiesId);
            return new EvaluateResponseModel
            {
                Result = result
            };
        }
    }
}
