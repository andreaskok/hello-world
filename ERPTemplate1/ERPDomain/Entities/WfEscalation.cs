using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfEscalation
    {
        public int ID { get; set; }
        public int SH_USERID { get; set; } //Foreign key to parent table SH_USER
        public double Mandate { get; set; }
        public int EscalationGroupID { get; set; }
        public virtual SH_USER SH_USER { get; set; } //Navigation property to parent table SH_USER
    }
}
