using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Segmentation.Domain.Abstractions
{
    public interface IExpressionCompilationService
    {
        Func<object, bool?> Compile(Segment segment);
    }
}
