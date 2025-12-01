using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Segmentation.Domain.Abstractions
{
    public interface ISegmentAdminService
    {
        Task Init();
        Task<Guid> Add(Segment segment, CancellationToken token);
        Task Update(Segment segment, CancellationToken token);
        Task Delete(Guid id, CancellationToken token);
        Task<IEnumerable<Segment>> GetPage(int pageNumber, int pageSize, CancellationToken token);
        Task<Segment> Get(Guid id, CancellationToken token);
    }
}
