using APIAUTH.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAUTH.Domain.Entities
{
    public class Collaborator : BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public int DocumentNamber { get; set; }
        public string DocumentType { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int CollaboratorTypeId { get; set; }
        public CollaboratorType CollaboratorType { get; set; }


        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
