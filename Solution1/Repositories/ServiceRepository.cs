using Microsoft.Data.SqlClient;
using Models;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ServiceRepository 
    {
        private ConnectionVsDb connectionVsDb;
        public ServiceRepository(ConnectionVsDb connectionVsDb)
        {
            this.connectionVsDb = connectionVsDb;
        }
        
        public async Task<string> GetServiceNamesByIds(List<Guid> listIds)
        {
            string query = "SELECT ServiceName FROM Service WHERE Id IN ({0})";
            string formattedIds = string.Join(",", listIds.Select(id => $"'{id}'"));
            query = string.Format(query, formattedIds);
            List<string> serviceNames = new List<string>();

            using (var connection = connectionVsDb.GetConnection())
            {
                await connection.OpenAsync();

                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string name = reader["ServiceName"].ToString();
                            serviceNames.Add(name);
                        }
                    }
                }
            }

            string result = string.Join(",", serviceNames);
            return result;
        }
        public async Task<List<Guid>> GetIdsByServiceNames(string serviceNames)
        {
            string query = "SELECT Id FROM Service WHERE ServiceName IN ({0})";
            List<Guid> ids = new List<Guid>();
            string formattedServiceNames = string.Join(",", serviceNames.Split(',').Select(name => $"'{name.Trim()}'"));
            query = string.Format(query, formattedServiceNames);

            using (var connection = connectionVsDb.GetConnection())
            {
                await connection.OpenAsync();

                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;

                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Guid id = reader.GetGuid(reader.GetOrdinal("Id"));
                            ids.Add(id);
                        }
                    }
                }
            }

            return ids;
        }
    }
}
