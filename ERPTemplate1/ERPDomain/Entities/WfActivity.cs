using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfActivity
    {
        public int ID { get; set; }
        public int WfActivityTypeID { get; set; } //Foreign key to parent table WfActivityType
        public int WfProcessID { get; set; } //Foreign key to parent table WfProcess
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual WfActivityType WfActivityType { get; set; } //Navigation property to parent table WfActivityType
        public virtual WfProcess WfProcess { get; set; } //Navigation property to parent table WfProcess
        public virtual ICollection<WfStateActivity> WfStateActivity { get; set; } //Navigation property to child table WfStateActivity
        public virtual ICollection<WfTransitionActivity> WfTransitionActivity { get; set; } //Navgation property to child table WfTransitionActivity
        public virtual ICollection<WfActivityType> WfActivityTarget { get; set; } //Navigation property to child table WfActivityTarget

    }
}
