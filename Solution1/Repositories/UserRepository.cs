using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository : DataBaseServiceBase<User>
    {
        public UserRepository(ConnectionVsDb connectionVsDb) : base(connectionVsDb)
        {
        }
        public async Task<Tuple<List<UserDto>,int>> FindUserByIdName(string name, int currentPage, int pageSize)
        {
            try
            {
                string query = $"SELECT * FROM [User] WHERE UserIdName LIKE @Name ORDER BY Id OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
                string countQuery = $"SELECT COUNT(*) FROM [User] WHERE UserIdName LIKE @Name";

                var nameParameter = connectionVsDb.CreateDbParameter("@Name", $"%{name}%");
                var offsetParameter = connectionVsDb.CreateDbParameter("@Offset", (currentPage - 1) * pageSize);
                var pageSizeParameter = connectionVsDb.CreateDbParameter("@PageSize", pageSize);

                using (var connection = connectionVsDb.GetConnection())
                {
                    await connection.OpenAsync();

                    List<UserDto> users = new List<UserDto>();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.Parameters.Add(nameParameter);
                        command.Parameters.Add(offsetParameter);
                        command.Parameters.Add(pageSizeParameter);


                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                User user = new User
                                {
                                    Id = (Guid)reader["Id"],
                                    UserIdName = (string)reader["UserIdName"],
                                    Role = (int)reader["Role"],
                                    Status = (int)reader["Status"],
                                    SigninMethod = (int)reader["SigninMethod"],
                                    ServiceNames = (string)reader["ServiceNames"]
                                };
                                UserDto userDto = user.UserToDTO();
                                users.Add(userDto);
                            }
                        }
                        command.Parameters.Clear();
                    }
                    int totalRecord;
                    using (var countCommand = connection.CreateCommand())
                    {
                        countCommand.CommandText = countQuery;
                        countCommand.Parameters.Add(nameParameter);

                        totalRecord = (int)await countCommand.ExecuteScalarAsync();
                       
                    }
                    int totalPage = totalRecord / pageSize +1;
                    return new Tuple<List<UserDto>, int> (users, totalPage );
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
        public async Task<List<UserDto>> FilterAsync(UserDto userDto, int currentPage, int pageSize)
        {
            {
                try
                {
                    User user=userDto.DtoToUser();
                    string query = $"SELECT * FROM [User] WHERE 1>0";


                    string query = $"SELECT * FROM [User] WHERE Status = @Status AND Role=@Role AND SigninMethod=@SigninMethod " +
                        $"OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ";
                    if(!string.IsNullOrWhiteSpace()) 

                    var roleParameter = connectionVsDb.CreateDbParameter("@Role", user.Role);
                    var statusParameter= connectionVsDb.CreateDbParameter("@Status", user.Status);
                    var signinMethodParameter = connectionVsDb.CreateDbParameter("@SigninMethod", user.SigninMethod);
                    var offsetParameter = connectionVsDb.CreateDbParameter("@Offset", (currentPage - 1) * pageSize);
                    var pageSizeParameter = connectionVsDb.CreateDbParameter("@PageSize", pageSize);

                    using (var connection = connectionVsDb.GetConnection())
                    {
                        await connection.OpenAsync();
                        using (var command = connection.CreateCommand())
                        {
                            command.CommandText = query;
                            command.Parameters.Add(nameParameter);
                            command.Parameters.Add(offsetParameter);
                            command.Parameters.Add(pageSizeParameter);

                            List<User> filteredUsers = new List<User>();

                            using (var reader = await command.ExecuteReaderAsync())
                            {
                                while (reader.Read())
                                {
                                    User filteredUser = new User
                                    {
                                        Id = (Guid)reader["Id"],
                                        Name = (string)reader["Name"],
                                        // Set other properties accordingly
                                    };

                                    filteredUsers.Add(filteredUser);
                                }
                            }

                            return filteredUsers;
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
