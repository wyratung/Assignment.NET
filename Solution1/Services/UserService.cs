
using Models;
using log4net;
using Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly ServiceUserCombineRepository _serviceUserCombineRepository;
        private DataBaseServiceBase<User> @object;
        private readonly ILogger _logger;
        public UserService(ConnectionVsDb connectionVsDb)
        {
            _userRepository = new UserRepository(connectionVsDb);
            _serviceRepository = new ServiceRepository(connectionVsDb);
            _serviceUserCombineRepository = new ServiceUserCombineRepository(connectionVsDb);
            _logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }

        public UserService(DataBaseServiceBase<User> @object)
        {
            this.@object = @object;
        }
        
        public async Task<int> CreateUserAsync(UserDto userDto)
        {
            
            try
            {
                _logger.Information("Creating user: {@User}", userDto);
                Console.WriteLine($"Creating user: Id:{userDto.UserIdName}");
                string? serviceNames = await _serviceRepository.GetServiceNamesByIds(userDto.listIdOfServices);
                userDto.ServiceNames=serviceNames;
                var id = Guid.NewGuid();
                userDto.Id = id;
                User user= userDto.DtoToUser();
                int userId = await _userRepository.CreateAsync(user);
                string? listServiceNames = user.ServiceNames;
                if (userDto.listIdOfServices != null)
                {                    
                    foreach(Guid IdService in userDto.listIdOfServices)
                    {
                        await _serviceUserCombineRepository.CreateAsync(new ServiceUserCombine(id,IdService));
                    }
                    
                }                                 

                
                Console.WriteLine($"User created successfully. UserIdName: {user.UserIdName}");
                _logger.Information("User created successfully. UserIdName: {UserIdName}", user.UserIdName);
                return userId;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating user");
                Console.WriteLine("Error creating user");
                throw;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            try
            {
                _logger.Information("Fetching user by Id: {Id}", id);
                Console.WriteLine($"Fetching user by Id: {id}");
                User user = await _userRepository.GetByIdAsync(id);
                if (user != null)
                {
                    _logger.Information("User found: {@User}", user);
                    Console.WriteLine($"User found:  Id:{id}");
                }
                else
                {
                    _logger.Information("User not found. Id: {Id}", id);
                    Console.WriteLine($"User not found. Id: {id}");
                }
                return user.UserToDTO();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching user by Id: {Id}", id);
                Console.WriteLine($"Error fetching user by Id: {id}");
                throw;
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                _logger.Information("Fetching all users");
                Console.WriteLine("Fetching all users");
                List<User> users = await _userRepository.GetAllAsync();
                List<UserDto> userDtos=users.ToList().Select(x=>x.UserToDTO()).ToList();
                _logger.Information("Total users fetched: {UserCount}", users.Count);
                Console.WriteLine($"Total users fetched: {users.Count}");
                return userDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching all users");
                Console.WriteLine("Error fetching all users");
                throw;
            }
        }

        public async Task<int> UpdateUserAsync(UserDto userDto)
        {
            
            var beforeUser = await _userRepository.GetByIdAsync(userDto.Id);
            IEnumerable<Guid> beforeListIdOfServices = await _serviceRepository.GetIdsByServiceNames(beforeUser.ServiceNames);
            IEnumerable<Guid>? addedService = userDto.listIdOfServices.ToList().Except(beforeListIdOfServices);
            if(addedService.Count()>0)
            {
                foreach (Guid IdService in addedService)
                {
                    await _serviceUserCombineRepository.CreateAsync(new ServiceUserCombine(userDto.Id, IdService));
                }
            }
            IEnumerable<Guid>? removedService = beforeListIdOfServices.ToList().Except(userDto.listIdOfServices);
            if (removedService.Count() > 0)
            {
                foreach (Guid IdService in removedService)
                {
                    await _serviceUserCombineRepository.DeleteAsync(userDto.Id, IdService);
                }
            }
            string? serviceNames = await _serviceRepository.GetServiceNamesByIds(userDto.listIdOfServices);
            userDto.ServiceNames = serviceNames;
            User user = userDto.DtoToUser();
            try
            {
                _logger.Information("Updating user: {@User}", user);
                Console.WriteLine($"Updating user:  Id:{user.Id}");
                int result = await _userRepository.UpdateAsync(user);
                _logger.Information("User updated successfully. UserIdName {UserIdName}", user.UserIdName);
                Console.WriteLine($"User updated successfully. UserIdName {user.UserIdName}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating user. UserIdName: {UserIdName}", user.UserIdName);
                Console.WriteLine($"Error updating user. UserIdName {user.UserIdName}");
                throw;
            }
        }

        public async Task<int> DeleteUserAsync(Guid id)
        {
            try
            {
                _logger.Information("Deleting user. Id: {Id}", id);           
                int result1= await _serviceUserCombineRepository.DeleteAsync(id);             

                int result2 = await _userRepository.DeleteAsync(id);
                _logger.Information("User deleted successfully. Id: {Id}", id);
                Console.WriteLine($"User deleted successfully. Id: {id}");
                return result1+ result2;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting user. Id: {Id}", id);
                Console.WriteLine($"Error deleting user. Id: {id}");
                throw;
            }
        }

        public Task<List<UserDto>> FilterAsync(UserDto userDto, int currentPage, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<List<UserDto>, int>> FindUserByIdName(string name, int currentPage, int pageSize)
        {
            return await _userRepository.FindUserByIdName(name, currentPage, pageSize);
        }

        public Task<bool> UpdateMuitipleUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateMuitipleUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMuitipleUserAsync(string[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeactivatedMuitipleUserAsync(string[] ids)
        {
            throw new NotImplementedException();
        }
    }
}
/*namespace Services
{
    public class UserService : IUserService
    {
        private readonly DataBaseServiceBase<User> _userRepository;
        private DataBaseServiceBase<User> @object;
        private readonly ILog _logger;

        public UserService(string connectionString, string dbType)
        {
            _userRepository = new DataBaseServiceBase<User>(connectionString, dbType);
            _logger = LogManager.GetLogger(typeof(UserService));
        }

        public UserService(DataBaseServiceBase<User> @object)
        {
            this.@object = @object;
            _logger = LogManager.GetLogger(typeof(UserService));
        }

        public async Task<int> CreateUserAsync(User user)
        {
            try
            {
                _logger.InfoFormat("Creating user: {0}", user);
                Console.WriteLine($"Creating user: Id:{user.Id}");
                int userId = await _userRepository.CreateAsync(user);
                Console.WriteLine($"User created successfully. UserIdName: {user.UserIdName}");
                _logger.InfoFormat("User created successfully. UserIdName: {0}", user.UserIdName);
                return userId;
            }
            catch (Exception ex)
            {
                _logger.Error("Error creating user", ex);
                Console.WriteLine("Error creating user");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                _logger.InfoFormat("Fetching user by Id: {0}", id);
                Console.WriteLine($"Fetching user by Id: {id}");
                User user = await _userRepository.GetByIdAsync(id);
                if (user != null)
                {
                    _logger.InfoFormat("User found: {0}", user);
                    Console.WriteLine($"User found:  Id:{id}");
                }
                else
                {
                    _logger.InfoFormat("User not found. Id: {0}", id);
                    Console.WriteLine($"User not found. Id: {id}");
                }
                return user;
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Error fetching user by Id: {0}", ex, id);
                Console.WriteLine($"Error fetching user by Id: {id}");
                throw;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                _logger.Info("Fetching all users");
                Console.WriteLine("Fetching all users");
                List<User> users = await _userRepository.GetAllAsync();
                _logger.InfoFormat("Total users fetched: {0}", users.Count);
                Console.WriteLine($"Total users fetched: {users.Count}");
                return users;
            }
            catch (Exception ex)
            {
                _logger.Error("Error fetching all users", ex);
                Console.WriteLine("Error fetching all users");
                throw;
            }
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            try
            {
                _logger.InfoFormat("Updating user: {0}", user);
                Console.WriteLine($"Updating user:  Id:{user.Id}");
                int result = await _userRepository.UpdateAsync(user);
                _logger.InfoFormat("User updated successfully. UserIdName {0}", user.UserIdName);
                Console.WriteLine($"User updated successfully. UserIdName {user.UserIdName}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Error updating user. UserIdName: {0}", ex, user.UserIdName);
                Console.WriteLine($"Error updating user. UserIdName {user.UserIdName}");
                throw;
            }
        }

        public async Task<int> DeleteUserAsync(int id)
        {
            try
            {
                _logger.InfoFormat("Deleting user. Id: {0}", id);
                Console.WriteLine($"Deleting user. Id: {id}");
                int result = await _userRepository.DeleteAsync(id);
                _logger.InfoFormat("User deleted successfully. Id: {0}", id);
                Console.WriteLine($"User deleted successfully. Id: {id}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Error deleting user. Id: {0}", ex, id);
                Console.WriteLine($"Error deleting user. Id: {id}");
                throw;
            }
        }

        public Task<List<User>> FilterByIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}*/