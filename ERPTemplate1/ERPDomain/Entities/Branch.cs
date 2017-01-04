using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Branch
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 OrganizationID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Status { get; set; }

        //public virtual Organization Organization { get; set; }

    }
}
