using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class MonthEndTransaction
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 OrganizationID { get; set; }
        public Int64 VoucherID { get; set; } 
        public Int64 VoucherLineID { get; set; } 
        public string VoucherCode { get; set; }
        public string VoucherLineCode { get; set; }
        public string Description { get; set; }
        public string AccYear { get; set; }
        public string AccMonth { get; set; }
        public string AccCode { get; set; }
        public string TransactType { get; set; }
        public DateTime DocDate { get; set; }
        public Double DocAmt { get; set; }
        public int Quantity { get; set; }
        public Double UnitPrice { get; set; }
        public Double Total { get; set; }
        public string DebitCreditIndicator { get; set; }
        public string PostingTable { get; set; }
        public DateTime PostingDate { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual JournalLine JournalLine { get; set; }

    }
}
