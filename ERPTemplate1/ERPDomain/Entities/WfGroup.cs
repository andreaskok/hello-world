using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfGroup
    {
        public int ID { get; set; }
        public int WfProcessID { get; set; } //Foreign key to table WfProcess
        public string Name { get; set; }

        public virtual WfProcess WfProcess { get; set; } // Navigation property to parent table WfProcess
        public virtual ICollection<WfGroupMember> WfGroupMember { get; set; } //Navigation property to child table WfGroupMember
        public virtual ICollection<WfActionTarget> WfActionTarget { get; set; } //Navigation property to child table WfActionTarget
        public virtual ICollection<WfActivityTarget> WfActivityTarget { get; set; } //Navigation property to child table WfActivityTarget
    }
}
