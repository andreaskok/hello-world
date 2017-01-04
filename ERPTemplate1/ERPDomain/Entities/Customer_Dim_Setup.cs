using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Customer_Dim_Setup
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 CustomerID { get; set; } //Foreign Key 
        public Int64 Dimension_SettingID { get; set; } //Foreign Key 
        public string Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Dimension_Setting Dimension_Setting { get; set; }
        public virtual ICollection<Customer_Dim_Value> Customer_Dim_Value { get; set; }

    }
}
