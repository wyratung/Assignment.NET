using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("User")]
    public class User
    {
        [Column("Id")]
        public Guid Id{ get; set; }
        [Column("UserIdName")]
        public string UserIdName { get; set; }
        [Column("Role")]
        public int Role { get; set; }
        [Column("Status")]
        public int Status { get; set; }
        [Column("SiginMethod")]
        public int SigninMethod { get; set; }
        [Column("ServiceNames")]
        public string ServiceNames { get; set; }

        public User() { }
        public User(string userIdName, int roleId, int statusId, int signinMethodId,string serviceNames)
        {
            ServiceNames = serviceNames;
            UserIdName = userIdName;
            Role = roleId;

            Status = statusId;

            SigninMethod = signinMethodId;

        }
        public UserDto UserToDTO()
        {
            UserDto userDto = new UserDto();
            userDto.Id = Id;
            userDto.UserIdName = UserIdName;
            userDto.ServiceNames = ServiceNames;
            userDto.StatusName = ((Role)Role).ToString();
            userDto.StatusName=((Status)Status).ToString();
            userDto.SigninMethodName=((SigninMethod)SigninMethod).ToString();
            return userDto;
        }
    }

}
