using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfGroupMember
    {
        public int ID { get; set; }
        public int WfGroupID { get; set; } //Foreign key to table WfGroup
        public int SH_USERID { get; set; }//Foreign key to table SH_USER

        public virtual WfGroup WfGroup { get; set; } //Navigation property to parent table WfGroup
        public virtual SH_USER SH_USER { get; set; } //Navigation property to parent table SH_USER
    }
}
