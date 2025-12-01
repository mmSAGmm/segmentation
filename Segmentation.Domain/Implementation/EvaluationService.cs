using Microsoft.Extensions.Logging;
using Segmentation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace Segmentation.Domain.Implementation
{
    public class EvaluationService(
        ISegmentAdminService segmentAdminService, 
        IPropertiesService propertiesService,
        IExpressionService expressionService,
        ILogger<EvaluationService> logger) : IEvaluationService
    {
        public async Task<bool?> Evaluate(Guid segmentId, string propertiesId)
        {
            var segment = await segmentAdminService.Get(segmentId);
            var properties = await propertiesService.Get(propertiesId);
            var dynamicProperties = ToExpando(properties);
            var lamda = expressionService.Parse(segment);
            bool? result = null;
            try
            {
                result = lamda(dynamicProperties);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "Failed on lamda execution");
            }
            return result;
        }

        private static dynamic ToExpando(Dictionary<string, object> source)
        {
            if (source == null)
            {
                return new ExpandoObject();
            }

            IDictionary<string, object> target = new ExpandoObject();
            foreach (var kvp in source)
            {
                target[kvp.Key] = kvp.Value;
            }

            return (ExpandoObject)target;
        }
    }
}
