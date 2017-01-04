using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Dimension_Value
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 Dimension_SettingID { get; set; } //Foreign Key 
        public string DimensionValue { get; set; }
        public string Status { get; set; }

        public virtual Dimension_Setting Dimension_Setting { get; set; }

    }
}
