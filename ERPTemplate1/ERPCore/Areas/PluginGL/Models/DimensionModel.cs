using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPCore.Models
{
    public class DimensionModel
    {
        public Int64 DimensionTableRelationshipID { get; set; }
        public Int64 DimensionSettingID { get; set; }
        public string DimensionCode { get; set; }
        public string DimensionTable { get; set; }
        public string DimensionSettingDescription { get; set; }

    }
}