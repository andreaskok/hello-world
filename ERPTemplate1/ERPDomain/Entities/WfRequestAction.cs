using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfRequestAction
    {
        public long ID { get; set; }
        public long WfRequestID { get; set; } //Foreign key to parent table WfRequest
        public int WfActionID { get; set; } //Foreign key to parent table WfAction
        public int WfTransitionID { get; set; } //Foreign key to parent table WfTransition

        public bool IsActive { get; set; }
        public bool IsComplete { get; set; }
        public int UserID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual WfRequest WfRequest { get; set; } //Navigation property to parent table WfRequest
        public virtual WfAction WfAction { get; set; } //Navigation property to parent table WfAction
        public virtual WfTransition WfTransition { get; set; }//Navigation property to parent table WfTransition
    }
}
