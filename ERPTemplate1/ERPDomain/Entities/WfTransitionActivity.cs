using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfTransitionActivity
    {
        public int ID { get; set; }
        public int WfTransitionID { get; set; }
        public int WfActivityID { get; set; }
        public virtual WfActivity WfActivity { get; set; } //Navigation property to parent table WfActivity
        public virtual WfTransition WfTransition { get; set; }// Navigation property to parent table WfTransition
    }
}
