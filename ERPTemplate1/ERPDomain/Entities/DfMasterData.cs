using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class DfMasterData
    {
        public int ID { get; set; }
        public int DfMasterID { get; set; } //Foreign key to parent table DfMaster
        public string Name { get; set; }
        public String Value { get; set; }
        public Int64 DfItemTypeID { get; set; }
        public virtual DfMaster DfMaster { get; set; } //Navigation property to parent table DfMaster
    }
}
