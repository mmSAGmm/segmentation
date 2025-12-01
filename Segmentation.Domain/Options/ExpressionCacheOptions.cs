using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.Domain.Options
{
    public class ExpressionCacheOptions
    {
        public TimeSpan Ttl { get; set; } = TimeSpan.FromMinutes(5);
    }
}
