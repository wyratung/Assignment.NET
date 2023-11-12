using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IServiceService
    {
        public Task<int> CreateServiceAsync(Service service);


        public Task<UserDto> GetServiceByIdAsync(Guid id);



        public Task<List<Service>> GetAllUsersAsync();
    }
}
