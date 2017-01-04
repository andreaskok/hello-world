using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Dimension_Setting
    {
        [Key]
        public Int64 ID { get; set; }

        public string DimensionCode { get; set; }
        public string Description { get; set; }
        public string DimensionType { get; set; }
        public string Status { get; set; }
        public string DimensionUsage { get; set; }
        public string PredefinedValue { get; set; }
        public virtual ICollection<Dimension_Value> Dimension_Value { get; set; }
        public virtual ICollection<Dimension_TableRelationship> Dimension_TableRelationship { get; set; }

    }
}
