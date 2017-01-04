using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfTransition
    {
        public int ID { get; set; }
        public int WfProcessID { get; set; } // Foreign key to parent table WfProcess
        public int CurrentStateID { get; set; }
        public int NextStateID { get; set; }

        public virtual WfProcess WfProcess { get; set; }
        public virtual ICollection<WfTransitionAction> WfTransitionAction { get; set; } //Navigation property to child table WfTransitionAction
        public virtual ICollection<WfTransitionActivity> WfTransitionActivity { get; set; } //navigation property to child table WfTransitionActivity
        public virtual ICollection<WfRequestAction> WfRequestAction { get; set; } //Navigation property to child table WfRequestAction

    }
}
