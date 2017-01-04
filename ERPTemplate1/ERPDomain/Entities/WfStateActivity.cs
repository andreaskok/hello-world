using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfStateActivity
    {
        public int ID { get; set; }
        public int WfStateID { get; set; } //Foreign key to parent table WfState
        public int WfActivityID { get; set; } //Foreign key to parent table WfActivity
        public virtual WfState WfState { get; set; } //Navigation property to parent table WfState
        public virtual WfActivity WfActivity { get; set; } //Navigation property to parent table WfActivity
    }
}
