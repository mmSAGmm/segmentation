using AutoMapper;
using Dapper;
using Microsoft.Extensions.Options;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DataAccess.Models;
using Segmentation.DataAccess.Options;
using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Segmentation.DataAccess.Implementation
{
    internal class SegmentsRepository(
        IConnectionProvider connectionProvider,
        IMapper mapper,
        IOptions<QueryOption> option) : ISegmentsRepository
    {

        private DbConnection Connection => connectionProvider.Get();

        public async Task<Guid> Add(Segment segment, CancellationToken token)
        {
            await Connection.ExecuteAsync($"INSERT INTO Segments(Id, Expression, Name) VALUES(@Id, @Expression, @Name)", segment, commandTimeout: option.Value.TimeoutSeconds);
            return segment.Id;
        }

        public async Task Delete(Guid id, CancellationToken token)
        {
            await Connection.ExecuteAsync($"DELETE FROM Segments WHERE Id = @Id", 
                new { Id = id },
                commandTimeout: option.Value.TimeoutSeconds);
        }

        public async Task<Segment> Get(Guid id, CancellationToken token)
        {
            var result = await Connection.QueryFirstOrDefaultAsync<SegmentDbModel>($"SELECT * FROM Segments WHERE Id = @Id", 
                new { Id = id },
                commandTimeout: option.Value.TimeoutSeconds);
            return mapper.Map<Segment>(result);
        }

        public async Task<IEnumerable<Segment>> GetPage(int pageNumber, int pageSize, CancellationToken token)
        {
            pageNumber = Math.Max(pageNumber - 1, 0);
            var result = await Connection.QueryAsync<SegmentDbModel>(
                $"SELECT * FROM Segments LIMIT {pageSize} OFFSET {pageNumber * pageSize};",
                commandTimeout: option.Value.TimeoutSeconds);

            return result.Select(x => mapper.Map<Segment>(x));
        }

        public async Task Init()
        {
            await Connection.ExecuteAsync(@$"
DROP TABLE Segments;
CREATE TABLE Segments (
    Id VARCHAR(20) PRIMARY KEY,
    Name VARCHAR(20),
    Expression TEXT NOT NULL
);");
        }

        public async Task Update(Segment segment, CancellationToken token)
        {
            await Connection.ExecuteAsync($"Update Segments SET Expression = @Expression, Name = @Name WHERE Id = @Id",
                segment,
                commandTimeout: option.Value.TimeoutSeconds);
        }
    }
}
