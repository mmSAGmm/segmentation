using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Segmentation.DomainModels;

namespace Segmentation.DataAccess.Abstraction
{
    public interface ISegmentsRepository
    {
        public Task Add(Segment segment);
    }
}
