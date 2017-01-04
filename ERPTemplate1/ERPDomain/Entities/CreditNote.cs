using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class CreditNote
    {

        [Key]
        public Int64 ID { get; set; }

        public string CreditNoteCode { get; set; }
        public string DocType { get; set; }
        public string CustomerCode { get; set; }
        public string TotalAmount { get; set; }
        public string Remark { get; set; }
        public string LocCode { get; set; }
        public string AccMonth { get; set; }
        public string AccYear { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime PrintDate { get; set; }
        public string UpdateID { get; set; }
        public Double OutstandingAmount { get; set; }
        public string CustRef { get; set; }
        public string DeliveryRef { get; set; }
        public DateTime PostDate { get; set; }
        public Double CurrencyRate { get; set; }
        public Double CurrencyAmount { get; set; }
        public Double CurrencyOutAmount { get; set; }
        public DateTime CreditNoteDate { get; set; }
        public string InvoiceCode { get; set; }

        public virtual ICollection<CreditNoteLine> CreditNoteLine { get; set; }
    }
}
