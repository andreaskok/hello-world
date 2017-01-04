using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class AccountGroup
    {
        [Key]
        public Int64 ID { get; set; }

        public string AccGrpCode { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateID { get; set; }
        public string AccClsCode { get; set; }

        public virtual ICollection<ChartOfAccount> ChartOfAccount { get; set; }

    }
}
