using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("ServiceUserCombine")]
    public class ServiceUserCombine
    {
        [Column("UserId")]
        public Guid UserId { get; set; }
        [Column("ServiceId")]
        public Guid ServiceId { get; set; }
        public ServiceUserCombine(Guid userId, Guid serviceId)
        {
            this.UserId = userId;
            this.ServiceId = serviceId;
        }
    }
}
