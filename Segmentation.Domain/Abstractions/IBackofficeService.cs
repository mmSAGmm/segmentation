using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Segmentation.Domain.Abstractions
{
    public interface IBackofficeService
    {
        Task Init();
        Task<Guid> Add(Segment segment);
        Task Update(Segment segment);
        Task Delete(Guid id);
        Task<IEnumerable<Segment>> GetPage(int pageNumber, int pageSize);
        Task<Segment> Get(Guid id);
    }
}
