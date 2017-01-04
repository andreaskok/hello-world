using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class OrganizationBranchModel
    {
        public Organization Organization { get; set; }
        public IEnumerable<Branch> Branch { get; set; }
        public Country Country { get; set; }
        public CountryState CountryState { get; set; }
        public Currency Currency { get; set; }

    }
}
