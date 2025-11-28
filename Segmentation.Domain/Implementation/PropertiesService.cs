using Segmentation.DataAccess.Abstraction;
using Segmentation.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Segmentation.Domain.Implementation
{
    public class PropertiesService(IPropertiesRepository repository) : IPropertiesService
    {
        public async Task Set<T>(T value, string name, string id)
        {
            var properties = await repository.Get(id);
            properties[name] = value;
            await repository.Set(properties, id);
        }

        public async Task<Dictionary<string, object>> Get(string id) 
        {
            return await repository.Get(id);
        }

        public async Task Init()
        {
            await repository.Init();
        }

        public async Task Set(Dictionary<string, object> values, string id)
        {
            var properties = await repository.Get(id);
            foreach (var property in values)
            {
                properties[property.Key] = property.Value;
            }
            await repository.Set(properties, id);
        }
    }
}
