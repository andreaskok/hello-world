using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfActivityTarget
    {
        public int ID { get; set; }
        public int WfActivityID { get; set; } //Foreign key to parent table WfActivity
        public int WfTargetID { get; set; } //Foreign key to parent table WfTarget
        public int WfGroupID { get; set; } //Foreign key to parent table WfGroup
        public virtual WfActivity WfActivity { get; set; }//Navigation property to parent table WfActivity
        public virtual WfTarget WfTarget { get; set; } //Navigation property to parent table WfTarget
        public virtual WfGroup WfGroup { get; set; } //Navigation property to parent table WfGroup

    }
}
