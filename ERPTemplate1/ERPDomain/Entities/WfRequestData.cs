using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfRequestData
    {
        public long ID { get; set; }
        public long WfRequestID { get; set; } //Foreign key to table WfRequest
        public string Name { get; set; }
        public string Value { get; set; }
        public virtual WfRequest WfRequest { get; set; } //Navigation property to parent table WfRequest



    }
}
