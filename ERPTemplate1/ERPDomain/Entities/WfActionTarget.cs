using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfActionTarget
    {
        public int ID { get; set; }
        public int WfActionID { get; set; } //Foreign key to parent table WfAction
        public int WfTargetID { get; set; } //Foreign key to parent table WfTarget
        public int WfGroupID { get; set; } //Foreign key to parent table WfGroup
        public virtual WfAction WfAction { get; set; } //Navigation property to parent table WfAction
        public virtual WfTarget WfTarget { get; set; } //Navigation property to parent table WfTarget
        public virtual WfGroup WfGroup { get; set; } //Navigation property to parent table WfGroup

    }
}
