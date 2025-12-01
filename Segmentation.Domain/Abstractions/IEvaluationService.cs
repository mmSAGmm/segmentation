using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.Domain.Abstractions
{
    public interface IEvaluationService
    {
        Task<bool> Evaluate(Guid segmentId, string propertiesId);
    }
}
