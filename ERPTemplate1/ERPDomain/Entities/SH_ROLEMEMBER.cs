using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class SH_ROLEMEMBER
    {
        public int ID { get; set; }
        public int SH_ROLEID { get; set; } //Foreign Key to SH_ROLE
        public int SH_USERID { get; set; } //Foreign Key to SH_USER

        public virtual SH_ROLE SH_ROLE { get; set; } //Navigation Property to parent table SH_ROLE
        public virtual SH_USER SH_USER { get; set; } //Navigation Property to parent table SH_USER

    }
}
