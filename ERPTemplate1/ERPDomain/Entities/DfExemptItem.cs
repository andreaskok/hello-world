using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class DfExemptItem
    {
        public Int64 ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Exempt { get; set; }
    }
}
