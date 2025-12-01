using AutoMapper;
using Dapper;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DataAccess.Models;
using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Segmentation.DataAccess.Implementation
{
    internal class SegmentsRepository(
        IConnectionProvider connectionProvider,
        IMapper mapper) : ISegmentsRepository
    {

        private DbConnection Connection => connectionProvider.Get();

        public async Task<Guid> Add(Segment segment, CancellationToken token)
        {
            await Connection.ExecuteAsync($"INSERT INTO Segments(Id, Expression) VALUES(@Id, @Expression)", segment);
            return segment.Id;
        }

        public async Task Delete(Guid id, CancellationToken token)
        {
            await Connection.ExecuteAsync($"DELETE FROM Segments WHERE Id = @Id", new { Id = id });
        }

        public async Task<Segment> Get(Guid id, CancellationToken token)
        {
            var result = await Connection.QueryFirstOrDefaultAsync<SegmentDbModel>($"SELECT * FROM Segments WHERE Id = @Id", new { Id = id });
            return mapper.Map<Segment>(result);
        }

        public async Task<IEnumerable<Segment>> GetPage(int pageNumber, int pageSize, CancellationToken token)
        {
            pageNumber = Math.Max(pageNumber - 1, 0);
            var result = await Connection.QueryAsync<SegmentDbModel>($"SELECT * FROM Segments LIMIT {pageSize} OFFSET {pageNumber * pageSize};", token);
            return result.Select(x => mapper.Map<Segment>(x));
        }

        public async Task Init()
        {
            await Connection.ExecuteAsync(@$"
CREATE TABLE Segments (
    Id VARCHAR(20) PRIMARY KEY,
    Expression TEXT NOT NULL
);");
        }

        public async Task Update(Segment segment, CancellationToken token)
        {
            await Connection.ExecuteAsync($"Update Segments SET Expression = @Expression WHERE Id = @Id", segment);
        }
    }
}
