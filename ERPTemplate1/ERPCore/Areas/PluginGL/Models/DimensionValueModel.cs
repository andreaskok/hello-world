using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPCore.Models
{
    public class DimensionValueModel
    {
        public Int64 DimensionValueID { get; set; }
        public Int64 DimensionSettingID { get; set; }
        public Int64 ChartOfAccountID { get; set; }
        public string DimensionValue { get; set; }
        public string RequestedDimensionValue { get; set; }
    }
}