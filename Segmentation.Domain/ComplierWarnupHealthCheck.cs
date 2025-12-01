using Microsoft.Extensions.Diagnostics.HealthChecks;
using Segmentation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.Domain
{
    public class ComplierWarnupHealthCheck(IExpressionCompilationService compilationService) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            compilationService.Compile(new DomainModels.Segment() { Expression = " x.name == 1" });
            return HealthCheckResult.Healthy();
        }
    }
}
