using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class DfRequest//Df=Dynamic Formula
    {
        public Int64 ID { get; set; }
        public int DfMasterID { get; set; } //Foreign key to parent table DfMaster
        public string Title { get; set; }
        public DateTime DateRequested { get; set; }
        public int UserID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual DfMaster DfMaster { get; set; } //Navigation property to parent table DfMaster
        public virtual ICollection<DfRequestData> DfRequestData { get; set; }// Navigation property to child table DfRequestData
    }
}
