using Microsoft.Extensions.Logging;
using Segmentation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Segmentation.Domain.Implementation
{
    public class EvaluationService(
        ISegmentAdminService segmentAdminService, 
        IPropertiesService propertiesService,
        IExpressionCompilationService expressionService,
        IExpressionCache expressionCache,
        ILogger<EvaluationService> logger) : IEvaluationService
    {
        public async Task<bool?> Evaluate(Guid segmentId, string propertiesId, CancellationToken token)
        {
            var segment = await segmentAdminService.Get(segmentId, token);
            if (segment == null) return null;

            var properties = await propertiesService.Get(propertiesId, token);
            var dynamicProperties = ToExpando(properties);
            var lamda = expressionCache.Get(segment.Expression);
            if (lamda == null) 
            { 
                lamda = expressionService.Compile(segment);
                expressionCache.Set(segment.Expression, lamda);
            }
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
                var value = kvp.Value;
                if (value is System.Text.Json.JsonElement el) {
                    value = el.ValueKind switch {
                        JsonValueKind.String => el.GetString(),
                        JsonValueKind.Number => el.GetDecimal(),
                        JsonValueKind.True => el.GetBoolean(),
                        JsonValueKind.False => el.GetBoolean()
                    };
                }

                target[kvp.Key] = value;
            }

            return (ExpandoObject)target;
        }
    }
}
