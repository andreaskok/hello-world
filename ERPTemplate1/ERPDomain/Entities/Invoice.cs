using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Invoice
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 CustomerID { get; set; } // Foreign Key 
        public Int64 TaxCodeID { get; set; } // Foreign Key 
        public Int64 CurrencyID { get; set; } // Foreign Key 

        public string InvoiceCode { get; set; }
        public string LocCode { get; set; }
        public string DocType { get; set; }
        public string CustomerCode { get; set; }
        public Decimal OutstandingAmount { get; set; }
        public Decimal TotalAmount { get; set; }
        public string CustRef { get; set; }
        public string DeliveryRef { get; set; }
        public string Remark { get; set; }
        public string AccMonth { get; set; }
        public string AccYear { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime PrintDate { get; set; }
        public string UpdateID { get; set; }
        public DateTime PostDate { get; set; }
        public string ServiceTax { get; set; }
        public string SalesTax { get; set; }
        public Decimal GrandAmount { get; set; }
        public Decimal CurrencyRate { get; set; }
        public Decimal CurrencyAmount { get; set; }
        public Decimal CurrencyOutAmount { get; set; }
        [DataType(DataType.Date), Display(Name = "Invoice Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }
        public string SelfBilledRefNo { get; set; }
        public DateTime SelfBilledRefDate { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceRefNo { get; set; }

        public virtual ICollection<InvoiceLine> InvoiceLine { get; set; }

    }
}
