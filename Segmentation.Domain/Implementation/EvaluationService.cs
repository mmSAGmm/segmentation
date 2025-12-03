using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Segmentation.Domain.Abstractions;
using Segmentation.Domain.Models;
using Segmentation.Domain.Options;
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
        ILoggerFactory loggerFactory,
        IOptions<EvaluationOption> options,
        ILogger<EvaluationService> logger) : IEvaluationService
    {
        public async Task<bool?> Evaluate(Guid segmentId, string propertiesId, CancellationToken token)
        {
            var segment = await segmentAdminService.Get(segmentId, token);
            if (segment == null) return null;

            var properties = await propertiesService.Get(propertiesId, token);
            var dynamicProperties = SafeDynamic(properties);
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

        private dynamic SafeDynamic(Dictionary<string, object> source)
        {
            Dictionary<string, object> container = new Dictionary<string, object>();
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

                if (options.Value.UseTypeMissmatchWapper)
                {
                    container[kvp.Key] = new TypeMissmatchDynamicWrapper(value, loggerFactory.CreateLogger<TypeMissmatchDynamicWrapper>());
                }
                else 
                {
                    container[kvp.Key] = value;
                }
            }
            return new SafeDynamic(container);
        }
    }
}
