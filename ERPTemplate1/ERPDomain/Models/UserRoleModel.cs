using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class UserRoleModel
    {
        public SH_USER SH_USER { get; set; }
        public IEnumerable<SH_USERROLE> SH_USERROLE { get; set; }
        public IEnumerable<SH_USER> SH_USERS { get; set; }
        public IEnumerable<SH_ROLE> SH_ROLES { get; set; } // Get Roles per User
        public SH_ROLE SH_ROLE { get; set; } //Get Role Description
        public IEnumerable<SH_ROLEMEMBER> SH_ROLEMEMBERS { get; set; }
        public IEnumerable<SH_ROLEACCESS> SH_ROLEACCESSS { get; set; }


    }
}
