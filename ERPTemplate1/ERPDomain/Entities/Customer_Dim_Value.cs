using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Customer_Dim_Value
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 Customer_Dim_SetupID { get; set; } //Foreign Key 
        public string DimensionValue { get; set; }
        public string Status { get; set; }
        public virtual Customer_Dim_Setup Customer_Dim_Setup { get; set; }
    }
}
