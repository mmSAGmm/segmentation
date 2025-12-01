using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Segmentation.Domain.Abstractions
{
    public interface IExpressionService
    {
        Func<object, bool> Parse(Segment segment);
    }
}
