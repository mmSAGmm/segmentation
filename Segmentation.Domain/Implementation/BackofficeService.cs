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
        ILogger logger) : IBackofficeService
    {
        public async Task Add(Segment segment)
        {
            await repository.Add(segment);
            logger.LogInformation("Segment created {id}, {expression}", segment.Id, segment.Expression);
        }

        public Task Delete(Segment segment)
        {
            throw new NotImplementedException();
        }

        public Task<Segment> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Segment segment)
        {
            throw new NotImplementedException();
        }
    }
}
