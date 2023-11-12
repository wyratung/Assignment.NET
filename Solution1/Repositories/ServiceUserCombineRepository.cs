using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class ServiceUserCombineRepository : DataBaseServiceBase<ServiceUserCombine>
    {
        public ServiceUserCombineRepository(ConnectionVsDb connectionVsDb) : base(connectionVsDb)
        {
        }
        public async Task<int> DeleteAsync(Guid userId)
        {
            try
            {
                string tableName = typeof(ServiceUserCombine).Name;
                string query = $"DELETE FROM [{tableName}] WHERE UserId = @Id";
                var idParameter = connectionVsDb.CreateDbParameter("@Id", userId);

                using (var connection = connectionVsDb.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.Add(idParameter);
                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public async Task<int> DeleteAsync(Guid userId,Guid serviceId)
        {
            try
            {
                string tableName = typeof(ServiceUserCombine).Name;
                string query = $"DELETE FROM [{tableName}] WHERE UserId = @UserId AND ServiceId = @ServiceId";

                var userIdParameter = connectionVsDb.CreateDbParameter("@UserId", userId);
                var serviceIdParameter = connectionVsDb.CreateDbParameter("@ServiceId", serviceId);

                using (var connection = connectionVsDb.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.Add(userIdParameter);
                        command.Parameters.Add(serviceIdParameter);
                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
