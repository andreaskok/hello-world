using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class DfAppliedModule
    {
        public int ID { get; set; }
        public int DfMasterID { get; set; }
        public string AppliedModule { get; set; }
    }
}
