using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfState
    {
        public int ID { get; set; }
        public int WfStateTypeID { get; set; } //Foreign key to parent table WfStateType
        public int WfProcessID { get; set; } //Foreign key to parent table WfProcess
        public string Name { get; set; } 
        public string Description { get; set; }

        public virtual WfStateType WfStateType { get; set; } //Navigation property to parent table WfStateType
        public virtual WfProcess WfProcess { get; set; } //Navigation property to parent table WfProcess

        public virtual ICollection<WfStateActivity> WfStateActivity { get; set; }//Navibation property to child table WfStateActivity
    }
}
