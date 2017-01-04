using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class ChartOfAccount_DimensionValue
    {
        [Key]
        public Int64 ID { get; set; }

        //public Int64 ChartOfAccountID { get; set; } // Foreign Key 
        public string AccCode { get; set; }
        public string DimensionCode { get; set; }
        public string DimensionValue { get; set; }
        public string Status { get; set; }

        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        //public virtual DimensionSetup DimensionSetup { get; set; }

    }
}
