using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.DataAccess.Abstraction
{
    public interface IPropertiesRepository
    {
        Task<Dictionary<string, object>> Get(string id);

        Task Set(Dictionary<string, object> value, string id);

        Task Init();
    }
}
