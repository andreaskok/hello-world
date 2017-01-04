using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class DfMaster//Df = Dynamic Formula
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Purpose { get; set; }
        public Int64 CountryID { get; set; }
        public virtual ICollection<DfMasterData> DfMasterData { get; set; } //Navigation property to child table DfMasterData
    }
}
