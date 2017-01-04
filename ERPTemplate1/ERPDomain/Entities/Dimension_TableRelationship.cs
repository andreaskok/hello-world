﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Dimension_TableRelationship
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 Dimension_SettingID { get; set; }//Foreign Key 
        //public Int64 ChartOfAccountID { get; set; }//Foreign Key 
        public string DimensionTable { get; set; }
        public string Status { get; set; }        
        public bool Activate { get; set; }

        public virtual Dimension_Setting Dimension_Setting { get; set; }
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
    }
}
