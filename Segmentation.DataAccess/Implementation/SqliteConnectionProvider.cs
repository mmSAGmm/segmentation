using Microsoft.Data.Sqlite;
using Segmentation.DataAccess.Abstraction;
using Segmentation.DataAccess.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Segmentation.DataAccess.Implementation
{
    public class SqliteConnectionProvider(SQLiteOption option) : IConnectionProvider
    {
        public DbConnection Get() 
        {
            return new SqliteConnection(option.ConnectionString);
        }
    }
}
