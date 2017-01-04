using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Organization
    {
        [Key]
        public Int64 ID { get; set; }

        public String OrganizationCode { get; set; }
        public String OrganizationName { get; set; }
        public String Address { get; set; }
        public String PostCode { get; set; }
        public String State { get; set; }
        public String City { get; set; }
        public String CountryCode { get; set; }
        public String TelNo { get; set; }
        public String FaxNo { get; set; }
        public String EPFRef { get; set; }
        public String SocsoRef { get; set; }
        public String Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public String UpdateID { get; set; }
        public String LogoUrl { get; set; }
        public String CurrencyCode { get; set; }
        public String CompRegNo { get; set; }
        public String GSTRegNo { get; set; }
        public String GAFVersion { get; set; }
        public Int64 ParentOrganization { get; set; }
        public String OrganizationType { get; set; }
        public virtual ICollection<MonthEndTransaction> MonthEndTransaction { get; set; }

    }
}
