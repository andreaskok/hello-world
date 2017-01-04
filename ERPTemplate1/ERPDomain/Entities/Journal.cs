using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Journal
    {
        [Key]
        public Int64 ID { get; set; }

        public string JournalCode { get; set; }
        public string Description { get; set; }
        public string ReceiveFrom { get; set; }
        public string TransactType { get; set; }
        public string RefNo { get; set; }
        public DateTime DocDate { get; set; }
        public Double DocAmt { get; set; }
        public Double GrandTotal { get; set; }
        public string AccMonth { get; set; }
        public string AccYear { get; set; }
        public string LocCode { get; set; }
        public string Status { get; set; }
        public DateTime PrintDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateID { get; set; }
        public DateTime PostDate { get; set; }
        public string ReverseID { get; set; }
        public string ChargeFrom { get; set; }
        public string FromJrnID { get; set; }
        public bool IsPosted { get; set; }

        public virtual ICollection<JournalLine> JournalLine { get; set; }
        //public virtual ICollection<ChartOfAccount> ChartOfAccount { get; set; }


    }
}
