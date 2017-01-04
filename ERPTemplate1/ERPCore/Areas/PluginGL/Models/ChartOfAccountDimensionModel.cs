using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class ChartOfAccountDimensionModel
    {
        public ChartOfAccount ChartOfAccount { get; set; }
        public AccountGroup AccountGroup { get; set; }
        //public IEnumerable<AccountGroup> AccountGroup { get; set; }
        public IEnumerable<Dimension_TableRelationship> Dimension_TableRelationship { get; set; }
        //public Dimension_TableRelationship Dimension_TableRelationship { get; set; }

    }
}
