using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class DataBaseServiceBase<T> where T : class
    {
        
        protected ConnectionVsDb connectionVsDb { get; set; }
        public DataBaseServiceBase(ConnectionVsDb connectionVsDb)
        {
            this.connectionVsDb = connectionVsDb;
        }

        public async Task<int> CreateAsync(T entity)
        {
            try
            {
                string tableName = typeof(T).Name;
                string columns = string.Join(", ", GetColumnNames());
                string parameters = string.Join(", ", GetParameterNames());
                string query = $"INSERT INTO [{tableName}] ({columns}) VALUES ({parameters})";
                Console.WriteLine(query);
                using (var connection = connectionVsDb.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        AddParameters(command, entity);
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

        public async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                string tableName = typeof(T).Name;
                string query = $"SELECT * FROM [{tableName}] WHERE Id = @Id";
                var idParameter = connectionVsDb.CreateDbParameter("@Id", id);

                using (var connection = connectionVsDb.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.Add(idParameter);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return MapEntity(reader);
                            } 
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<List<T>> GetAllAsync()
        {
            try
            {
                string tableName = typeof(T).Name;
                string query = $"SELECT * FROM [{tableName}]";

                using (var connection = connectionVsDb.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            List<T> entities = new List<T>();
                            while (await reader.ReadAsync())
                            {
                                entities.Add(MapEntity(reader));
                            }
                            return entities;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<int> UpdateAsync(T entity)
        {
            try
            {
                string tableName = typeof(T).Name;
                string setClause = string.Join(", ", GetColumnNames().Select(c => $"{c} = @{c}"));
                string query = $"UPDATE [{tableName}] SET {setClause} WHERE Id = @Id";

                using (var connection = connectionVsDb.GetConnection())
                {
                    await connection.OpenAsync();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        AddParameters(command, entity);
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

        public async Task<int> DeleteAsync(Guid id)
        {
            try
            {
                string tableName = typeof(T).Name;
                string query = $"DELETE FROM [{tableName}] WHERE Id = @Id";
                var idParameter = connectionVsDb.CreateDbParameter("@Id", id);

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

       

        private List<string> GetColumnNames()
        {
            return typeof(T).GetProperties().Where(p => CheckExistColumnAttribute(p)).Select(p => p.Name).ToList();
        }
        private bool CheckExistColumnAttribute(PropertyInfo prop)
        {
            var checkAttribute = (ColumnAttribute?)Attribute.GetCustomAttribute(prop, typeof(ColumnAttribute));
            return checkAttribute != null;
        }
        private List<string> GetParameterNames()
        {
            return GetColumnNames().Select(c => $"@{c}").ToList();
        }

        private void AddParameters(DbCommand command, T entity)
        {
            var properties = typeof(T).GetProperties().Where(p => CheckExistColumnAttribute(p)).ToList();
            foreach (var property in properties)
            {
                var parameter = connectionVsDb.CreateDbParameter($"@{property.Name}", property.GetValue(entity));
                command.Parameters.Add(parameter);
            }
        }     

        private T MapEntity(IDataReader reader)
        {
            var entity = Activator.CreateInstance<T>();
            var properties = typeof(T).GetProperties().Where(p => CheckExistColumnAttribute(p));
            foreach (var property in properties)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                {
                    var value = reader[property.Name];
                    property.SetValue(entity, value);
                }
            }
            return entity;
        }
        
    }
}
