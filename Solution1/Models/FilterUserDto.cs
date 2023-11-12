using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class FilterUserDto
    {
        public string UserIdName { get; set; }
        public List<int> Role { get; set; }
        public List<int> Status { get; set; }
        public List<int> SigninMethodName { get; set; }
        public List<Guid> listIdOfServices { get; set; }
    }
}
