using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class DimensionSettingModel
    {
        public Dimension_Setting Dimension_Setting { get; set; }
        public IEnumerable<Dimension_Value> Dimension_Value { get; set; }
        public IEnumerable<Dimension_TableRelationship> Dimension_TableRelationship { get; set; }
    }
}
