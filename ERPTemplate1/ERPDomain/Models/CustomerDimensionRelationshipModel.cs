using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class CustomerDimensionRelationshipModel
    {
        public Customer Customer { get; set; }
        public IEnumerable<Dimension_TableRelationship> Dimension_TableRelationship { get; set; }
        public IEnumerable<Customer_Dim_Setup> Customer_Dim_Setup { get; set; }
        public IEnumerable<Customer_Dim_Value> Customer_Dim_Value { get; set; }

    }
}
