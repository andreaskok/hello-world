using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class DfRequestData
    {
        public Int64 ID { get; set; }
        public Int64 DfRequestID { get; set; } //Foreign key to parent table DfRequest
        public string Name { get; set; }
        public String Value { get; set; }
        public Int64 HeaderID { get; set; }
        public Int64 LineID { get; set; }
        public int TableID { get; set; }
        public virtual DfRequest DfRequest { get; set; }//Navigation property to parent table DfRequest
         
    }
}
