using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Segmentation.DataAccess.Abstraction
{
    public interface IConnectionProvider
    {
        DbConnection Get();
    }
}
