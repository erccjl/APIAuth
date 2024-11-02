using APIAUTH.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Aplication.DTOs
{
    public class BaseEntityDto : BaseDto
    {
        public DateTime? UpdatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public BaseState? State { get; set; }
    }
}
