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
    internal class BackofficeService(
        ISegmentsRepository repository,
        ILogger<BackofficeService> logger) : IBackofficeService
    {
        public async Task Init()
        {
            await repository.Init();
        }

        public async Task Add(Segment segment)
        {
            await repository.Add(segment);
            logger.LogInformation("Segment created {id}, {expression}", segment.Id, segment.Expression);
        }

        public async Task Delete(Guid id)
        {
            await repository.Delete(id);
            logger.LogInformation("Segment deleted {id}", id);
        }

        public async Task<Segment> Get(Guid id)
        {
            var segment = await repository.Get(id);
            logger.LogInformation("Segment retrieved {id}", id);
            return segment;
        }

        public async Task<IEnumerable<Segment>> GetPage(int pageNumber, int pageSize)
        {
            var segment = await repository.GetPage(pageNumber, pageSize);
            logger.LogInformation("Segment GetPage retrieved {pageNumber} {pageSize}", pageNumber, pageSize);
            return segment;
        }

        public async Task Update(Segment segment)
        {
            await repository.Update(segment);
            logger.LogInformation("Segment updated {id}, {expression}", segment.Id, segment.Expression);
        }
    }
}
