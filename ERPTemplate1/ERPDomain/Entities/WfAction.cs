using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfAction
    {
        public int ID { get; set; }
        public int WfActionTypeID { get; set; } //Foreign key to parent table WfActionType
        public int WfProcessID { get; set; } // Foreign key to parent table WfProcess
        public string Name { get; set; }
        public string Desciption { get; set; }
        public virtual WfActionType WfActionType { get; set; } //Navigation property to parent table WfActionType
        public virtual WfProcess WfProcess { get; set; } //Navigation property to parent table WfProcess
        public virtual ICollection<WfTransitionAction> WfTransitionAction { get; set; } //Navigation property to child table WfTransitionAction
        public virtual ICollection<WfActionTarget> WfActionTarget { get; set; } //Navigation property to child table WfActionTarget
        public virtual ICollection<WfRequestAction> WfRequestAction { get; set; } //Navigation property to child table WfRequestAction
    }
}
