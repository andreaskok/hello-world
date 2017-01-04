using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class ChartOfAccount_Dim_Value
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 ChartOfAccount_Dim_SetupID { get; set; } //Foreign Key 
        public string DimensionValue { get; set; }
        public string Status { get; set; }

        public virtual ChartOfAccount_Dim_Setup ChartOfAccount_Dim_Setup { get; set; }
    }
}
