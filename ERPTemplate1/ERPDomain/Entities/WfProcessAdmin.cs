using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfProcessAdmin
    {
        public int ID { get; set; }
        public int WfProcessID { get; set; } // Foreign Key to table WfProcess
        public int SH_USERID { get; set; } // Foreign Key to table SH_USER

        public virtual WfProcess WfProcess { get; set; } //Navigation property to parent table WfProcess

        public virtual SH_USER SH_USER { get; set; } //Navigation Property to parent table SH_USER

    }
}
