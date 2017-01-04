using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class SH_ROLE
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public bool AdminFlag { get; set; }
        public bool SystemFlag { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<SH_ROLEMEMBER> SH_ROLEMEMBER { get; set; }
        public virtual ICollection<SH_ROLEACCESS> SH_ROLEACCESS { get; set; }
        public virtual ICollection<SH_USERROLE> SH_USERROLE { get; set; }
    }
}
