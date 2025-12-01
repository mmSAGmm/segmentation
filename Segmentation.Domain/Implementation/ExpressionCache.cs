using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using Segmentation.Domain.Abstractions;
using Segmentation.Domain.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.Domain.Implementation
{
    internal class ExpressionCache(
        ISystemClock systemClock,
        IMemoryCache memoryCache,
        IOptions<ExpressionCacheOptions> option) : IExpressionCache
    {
        public Func<object, bool?>? Get(string expression)
        {
            return memoryCache.Get<Func<object, bool?>>(expression);
        }

        public void Set(string expression, Func<object, bool?> value)
        {
            memoryCache.Set(expression, value, systemClock.UtcNow.Add(option.Value.Ttl));
        }
    }
}
