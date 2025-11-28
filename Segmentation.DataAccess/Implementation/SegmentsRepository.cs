using Dapper;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Segmentation.DataAccess.Implementation
{
    internal class SegmentsRepository(IConnectionProvider connectionProvider) : ISegmentsRepository
    {
        private char @p => connectionProvider.ParametrPrefix();

        private DbConnection Connection => connectionProvider.Get();

        public async Task Add(Segment segment)
        {
            await Connection.ExecuteAsync($"INSERT INTO Segments(Id, Expression) VALUES({@p}Id, {@p}expression)");
        }

        public async Task Delete(Segment segment)
        {
            await Connection.ExecuteAsync($"DELETE FROM Segments WHERE Id = {@p}Id");
        }

        public async Task<Segment> Get(Guid id)
        {
            var result = await Connection.QueryFirstAsync<Segment>($"SELECT * FROM Segments WHERE Id = {@p}Id");
            return result;
        }

        public async Task<IEnumerable<Segment>> GetPage(int pageNumber, int pageSize)
        {
            pageNumber = Math.Max(pageNumber - 1, 0);
            return await Connection.QueryAsync<Segment>($"SELECT * FROM Segments LIMIT {pageSize} OFFSET {pageNumber * pageSize};");
        }

        public async Task Update(Segment segment)
        {
            await Connection.ExecuteAsync($"Update Segments SET Expression = {@p}expression WHERE Id = {@p}Id");
        }
    }
}
