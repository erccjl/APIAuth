using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Domain.Entities
{
    public class NotificationUser : BaseEntity
    {
        public int UserId { get; set; }
        public int NotificationId { get; set; }

        public bool Enable { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual User User { get; set; }
    }
}
