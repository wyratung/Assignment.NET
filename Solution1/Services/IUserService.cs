using Models;
using Org.BouncyCastle.Utilities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService 
    {


        public Task<int> CreateUserAsync(UserDto userDto);


        public  Task<UserDto> GetUserByIdAsync(Guid id);



        public Task<List<UserDto>> GetAllUsersAsync();



        public  Task<int> UpdateUserAsync(UserDto userDto);



        public Task<int> DeleteUserAsync(Guid id);

        public Task<List<UserDto>> FilterAsync(UserDto userDto, int currentPage, int pageSize);
        public Task<Tuple<List<UserDto>, int>> FindUserByIdName(string name, int currentPage, int pageSize);
        public Task<Boolean> UpdateMuitipleUserAsync();
        public Task<Boolean> CreateMuitipleUserAsync(User user);
        public Task<Boolean> DeleteMuitipleUserAsync(string[] ids);
        public Task<Boolean> DeactivatedMuitipleUserAsync(string[] ids);
    }

}
