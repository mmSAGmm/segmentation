using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DataAccess.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Segmentation.DataAccess.Implementation
{
    public class SqliteConnectionProvider(IOptions<SQLiteOption> option) : IConnectionProvider
    {
        public DbConnection Get() 
        {
            return new SqliteConnection(option.Value.ConnectionString);
        }
    }
}
