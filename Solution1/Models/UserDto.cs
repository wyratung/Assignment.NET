using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserIdName { get; set; }
        public string RoleName { get; set; }
        public string StatusName { get; set; }
        public string SigninMethodName { get; set; }
        public string ServiceNames { get; set; }
        public int Role {  get; set; }
        public int Status { get; set; }
        public int SigninMethod { get; set; }
        public List<int> ListRole { get; set; }
        public List<int> ListStatus { get; set; }
        public List<int> ListSigninMethodName { get; set; }
        public List<Guid> listIdOfServices {  get; set; }
        public User  DtoToUser()
        {
            User user = new User();
            user.UserIdName = UserIdName;
            user.Id = Id;
            user.ServiceNames = ServiceNames;
            user.Role = (int)((Role)Enum.Parse(typeof(Role), RoleName));
            user.Role = (int)((Role)Enum.Parse(typeof(Role), RoleName));
            user.Status = (int)((Status)Enum.Parse(typeof(Status), StatusName));
            user.SigninMethod=(int)((SigninMethod)Enum.Parse(typeof(SigninMethod), SigninMethodName));
            return user;
        }
    }
}
