using Dapper;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Segmentation.DataAccess.Implementation
{
    public class PropertiesRepository(IConnectionProvider provider) : IPropertiesRepository
    {
        private DbConnection Connection => provider.Get();

        public async Task<Dictionary<string, object>> Get(string id, CancellationToken token)
        {
            var result = await Connection.QueryFirstOrDefaultAsync<PropertiesDbModel>("SELECT * FROM Properties WHERE Id = @Id", new { Id = id });
            var json = result?.Json ?? "{}";
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }

        public async Task Set(Dictionary<string, object> value, string id, CancellationToken token)
        {
            using var connection = Connection;
            await connection.OpenAsync();
         //   using var transaction = await Connection.BeginTransactionAsync();

            await Connection.ExecuteAsync("DELETE FROM Properties WHERE Id = @Id", new { Id = id });
            await Connection.ExecuteAsync("INSERT INTO Properties VALUES (@Id, @Json)",
                new
                {
                    Id = id,
                    Json = JsonSerializer.Serialize(value)
                });
           // await transaction.CommitAsync();
        }

        public async Task Init() 
        {
            await Connection.ExecuteAsync(
                @"
CREATE TABLE Properties 
(
    Id VARCHAR(20) PRIMARY KEY,
    Json TEXT NOT NULL
);");
        }
    }
}
