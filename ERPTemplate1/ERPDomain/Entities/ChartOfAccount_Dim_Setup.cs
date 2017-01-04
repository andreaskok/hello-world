using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class ChartOfAccount_Dim_Setup
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 ChartOfAccountID { get; set; } //Foreign Key 
        public Int64 Dimension_SettingID { get; set; } //Foreign Key
        public string Status { get; set; }

        public virtual ChartOfAccount ChartOfAccount { get; set; } //Navigation Property to parent table ChartOfAccount
        public virtual Dimension_Setting Dimension_Setting { get; set; } //Navigation Property to parent table Dimension_Setting
        public virtual ICollection<ChartOfAccount_Dim_Value> ChartOfAccount_Dim_Value { get; set; }

    }
}
