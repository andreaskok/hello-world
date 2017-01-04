using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfRequestNote
    {
        public long ID { get; set; }
        public long WfRequestID { get; set; } //Foreign key to parent table WfRequest
        public int SH_USERID { get; set; } //Foreign key to parent table SH_USER
        public string Note { get; set; }
        public virtual WfRequest WfRequest { get; set; } //Navigation property to parent table WfRequest
        public virtual SH_USER SH_USER { get; set; } //Navigation property to parent table SH_USER
    }
}
