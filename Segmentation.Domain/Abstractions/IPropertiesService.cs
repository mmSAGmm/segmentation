using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.Domain.Abstractions
{
    public interface IPropertiesService
    {
        Task Set<T>(T value, string name, string id);
        Task Set(Dictionary<string, object> values, string id);
        Task<Dictionary<string, object>> Get(string id);
        Task Init();
    }
}
