using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Service")]
    public class Service
    {
        [Column("Id")]
        public Guid Id{ get; set; }
        [Column("ServiceName")]
        public string ServiceName { get; set; }
        public Service(string serviceName)
        {
            ServiceName = serviceName;
        }
    }

}
