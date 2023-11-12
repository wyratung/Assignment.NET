using Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceService : IServiceService
    {
        private readonly DataBaseServiceBase<Service> _serviceRepository;
        public ServiceService(ConnectionVsDb connectionVsDb)
        {
            _serviceRepository = new DataBaseServiceBase<Service>(connectionVsDb);
        }
        public Task<int> CreateServiceAsync(Service service)
        {
            try
            {
                Console.WriteLine("insert service");
                Guid serviceGuid = Guid.NewGuid();
                service.Id = serviceGuid;
                var result= _serviceRepository.CreateAsync(service);
                return result;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public Task<List<Service>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetServiceByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
