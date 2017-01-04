using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ERPCore.Models
{
    public class ViewDimensionValueModel
    {
        public List<DimensionValueModel> AvailableDimensionValues { get; set; }
        public List<DimensionValueModel> RequestedDimensionValues { get; set; }

        public long[] AvailableSelected { get; set; }
        public long[] RequestedSelected { get; set; }
        public string SavedRequested { get; set; }
        
    }
}