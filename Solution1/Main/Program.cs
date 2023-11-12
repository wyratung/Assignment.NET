using Models;
using MySqlX.XDevAPI.Common;
using Repositories;
using Services;
using System;
namespace Test
{
    public class Tesstt
    {
        static async Task Main(string[] args)
        {
            string connectionString = @"Data Source=(localdb)\ProjectModels;
                                            Initial Catalog=AdoTest;Integrated Security=True;
                                            Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;
                                            Application Intent=ReadWrite;Multi Subnet Failover=False";
            string typeDb = "SQLServer";

            /*UserDto user1 = new UserDto
            {
                //Id = Guid.Parse("118aec48-b680-490c-9239-c0412013e58b"),

                UserIdName = "TestAbc",
                StatusName = "Activated",
                SigninMethodName = "SalesforceUser",
                RoleName = "ServiceAdministrator",
                listIdOfServices = new List<Guid>
                    {
                        Guid.Parse("92d128b1-351a-455d-857a-1f84285dc191"),
                       Guid.Parse("4598edca-3c84-4c34-a2cb-6cfabbe7fc8a"),
                       Guid.Parse("6b9c2549-3870-4157-8188-eb8d6c8758d6"),
                       Guid.Parse("d52434f8-ef77-41f7-bf87-216b9b30a636")
                    }
            };*/
            var connectionDb = new ConnectionVsDb(connectionString, typeDb);
            //Service service = new Service("Refactor");
            IUserService userRepository = new UserService(connectionDb);
            var result=await userRepository.FindUserByIdName("13024", 1, 2);
            List<UserDto> users = result.Item1;

            foreach (var user in users)
            {
                Console.WriteLine($"User ID: {user.Id}");
                Console.WriteLine($"User Name: {user.UserIdName}");
                Console.WriteLine($"User Status: {user.StatusName}");
                Console.WriteLine($"User SigninMethod: {user.SigninMethodName}");
                Console.WriteLine("--------------------------------------");
            }
            Console.WriteLine($"Total Page: {result.Item2}");
            /* IServiceService serviceService = new ServiceService(connectionDb);
             await serviceService.CreateServiceAsync(service);*/
            //await userRepository.CreateUserAsync(user1);
            //await userRepository.DeleteUserAsync(Guid.Parse("a936e419-2903-49b5-89b2-3baffae516b5"));
        }
    }
   
}