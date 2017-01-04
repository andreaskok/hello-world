using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfTransitionAction
    {
        public int ID { get; set; }
        public int WfTransitionID { get; set; } //Foreign key to parent table WfTransition
        public int WfActionID { get; set; } //Foreign key to parent table WfAction

        public virtual WfTransition WfTransition { get; set; } //Navigation property to parent table WfTransition
        public virtual WfAction WfAction { get; set; } //Navigation property to parent table WfAction
    }
}
