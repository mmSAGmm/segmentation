using Segmentation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Segmentation.Domain.Implementation
{
    public class EvaluationService(
        ISegmentAdminService segmentAdminService, 
        IPropertiesService propertiesService,
        IExpressionService expressionService ) : IEvaluationService
    {
        public async Task<bool> Evaluate(Guid segmentId, string propertiesId)
        {
            var segment = await segmentAdminService.Get(segmentId);
            var properties = await propertiesService.Get(propertiesId);
            var a = properties as dynamic;
            var lamda = expressionService.Parse(segment);

            return lamda(a);

        }
    }
}
