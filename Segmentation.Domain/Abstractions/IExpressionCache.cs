using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.Domain.Abstractions
{
    public interface IExpressionCache
    {
        Func<object, bool?>? Get(string expression);

        void Set(string expression, Func<object, bool?> value);
    }
}
