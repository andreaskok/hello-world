using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class DimensionSetup
    {
        [Key]
        public Int64 ID { get; set; }
        public string DimensionTable { get; set; }
        public string DimensionCode { get; set; }
        public string DimensionType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        //public virtual ICollection<ChartOfAccount_DimensionValue> ChartOfAccount_DimensionValue { get; set; }

    }
}
