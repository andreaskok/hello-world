using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfProcess
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<WfProcessAdmin> WfProcessAdmin { get; set; } //Navigation Property to Child Table WfProcessAdmin
        public virtual ICollection<WfGroup> WfGroup { get; set; } //Navigation property to child table WfGroup
        public virtual ICollection<WfRequest> WfRequest { get; set; } //Navigation property to child table WfRequest
        public virtual ICollection<WfState> WfState { get; set; } //Navigation property to child table WfState
        public virtual ICollection<WfTransition> WfTransition { get; set; } //Navigation property to child table WfTransition
        public virtual ICollection<WfAction> WfAction { get; set; } //Navigaqqtion property to child table WfAction
        public virtual ICollection<WfActivity> WfActivity { get; set; } //Navigation property to child table WfActivity
    }
}
