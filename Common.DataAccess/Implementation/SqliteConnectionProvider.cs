using Common.DataAccess.Abstraction;
using Common.DataAccess.Options;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Common.DataAccess.Implementation
{
    public class SqliteConnectionProvider(IOptions<SQLiteOption> option) : IConnectionProvider
    {
        public DbConnection Get() 
        {
            return new SqliteConnection(option.Value.ConnectionString);
        }
    }
}
