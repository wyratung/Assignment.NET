using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ConnectionVsDb
    {
        private string connectionString;
        private string dbType;
        public ConnectionVsDb(string connectionString,string dbType)
        {
            this.connectionString = connectionString;
            this.dbType = dbType;
        }
        public DbConnection GetConnection()
        {

            if (dbType == "SQLServer")
            {
                return new SqlConnection(connectionString);
            }
            else if (dbType == "MySQL")
            {
                return new MySqlConnection(connectionString);
            }
            else if (dbType == "PostgreSQL")
            {
                return new NpgsqlConnection(connectionString);
            }
            else
            {
                throw new NotSupportedException($"Database type '{dbType}' is not supported.");
            }
        }
        public IDbDataParameter CreateDbParameter(string name, object value)
        {
            // Implement the logic to create and return a database parameter based on the dbType
            // For example:
            if (dbType == "SQLServer")
            {
                return new SqlParameter(name, value);
            }
            else if (dbType == "MySQL")
            {
                return new MySqlParameter(name, value);
            }
            else if (dbType == "PostgreSQL")
            {
                return new NpgsqlParameter(name, value);
            }
            else
            {
                throw new NotSupportedException($"Database type '{dbType}' is not supported.");
            }
        }
    }
}
