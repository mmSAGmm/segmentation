using Microsoft.Extensions.Logging;
using Segmentation.DataAccess.Abstraction;
using Segmentation.Domain.Abstractions;
using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Segmentation.Domain.Implementation
{
    internal class SegmentAdminService(
        ISegmentsRepository repository,
        ILogger<SegmentAdminService> logger) : ISegmentAdminService
    {
        public async Task Init()
        {
            await repository.Init();
        }

        public async Task<Guid> Add(Segment segment, CancellationToken token)
        {
            var id = await repository.Add(segment, token);
            logger.LogInformation("Segment created {id}, {expression}", segment.Id, segment.Expression);
            return id;
        }

        public async Task Delete(Guid id, CancellationToken token)
        {
            await repository.Delete(id, token);
            logger.LogInformation("Segment deleted {id}", id);
        }

        public async Task<Segment> Get(Guid id, CancellationToken token)
        {
            var segment = await repository.Get(id, token);
            logger.LogInformation("Segment retrieved {id}", id);
            return segment;
        }

        public async Task<IEnumerable<Segment>> GetPage(int pageNumber, int pageSize, CancellationToken token)
        {
            var segment = await repository.GetPage(pageNumber, pageSize, token);
            logger.LogInformation("Segment GetPage retrieved {pageNumber} {pageSize}", pageNumber, pageSize);
            return segment;
        }

        public async Task Update(Segment segment, CancellationToken token)
        {
            await repository.Update(segment, token);
            logger.LogInformation("Segment updated {id}, {expression}", segment.Id, segment.Expression);
        }
    }
}
