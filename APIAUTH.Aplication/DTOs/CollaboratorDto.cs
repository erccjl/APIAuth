using APIAUTH.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Aplication.DTOs
{
    public class CollaboratorDto : BaseEntityDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int DocumentNamber { get; set; }
        public string? DocumentType { get; set; }
        public string NumberPhone { get; set; }
        public string? Photo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string Email { get; set; }
        public string BackupEmail { get; set; }

        public int? CollaboratorTypeId { get; set; }
        //public CollaboratorTypeDto CollaboratorTypeEnum { get; set; }


        public int? OrganizationId { get; set; }
        //public OrganizationDto Organization { get; set; }

        public int UserId { get; set; }
        public UserDto? User { get; set; }

        public int RoleId   { get; set; }
        public RoleDto? Role { get; set; }
    }
}
