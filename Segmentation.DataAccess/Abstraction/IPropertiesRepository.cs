using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Segmentation.DataAccess.Abstraction
{
    public interface IPropertiesRepository
    {
        Task<Dictionary<string, object>> Get(string id, CancellationToken token);

        Task Set(Dictionary<string, object> value, string id, CancellationToken token);

        Task Init();
    }
}
