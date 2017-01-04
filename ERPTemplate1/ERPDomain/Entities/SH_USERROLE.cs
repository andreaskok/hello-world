using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class SH_USERROLE
    {
        public int ID { get; set; }
        public int SH_USERID { get; set; }
        public int SH_ROLEID { get; set; }
        public bool Assign { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual SH_USER SH_USER { get; set; } //Navigation Property to parent table SH_USER
        public virtual SH_ROLE SH_ROLE { get; set; } // Navigation Property to parent table SH_ROLE
    }
}
