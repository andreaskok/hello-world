using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Payment
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 VendorID { get; set; } // Foreign Key 
        public Int64 BankID { get; set; } // Foreign Key 
        public Int64 CurrencyID { get; set; } // Foreign Key 
        public string PaymentCode { get; set; }
        public string VendorCode { get; set; }
        public string PaymentType { get; set; }
        public string BankCode { get; set; }
        public string ChequeNo { get; set; }

        [Display(Name = "Total Amount")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.######}")]
        public Double TotalAmount { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }
        public string LocCode { get; set; }
        public string AccMonth { get; set; }
        public string AccYear { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime PrintDate { get; set; }
        public string UpdateID { get; set; }
        public DateTime ChequePrintDate { get; set; }
        public string OutstandingAmount { get; set; }
        public string CreditBankCode { get; set; }
        public DateTime CreditDate { get; set; }
        public DateTime PostDate { get; set; }
        public string ProposalID { get; set; }
        public string CurrencyRate { get; set; }
        public Decimal CurrencyAmount { get; set; }
        public Decimal CurrencyOutAmount { get; set; }
        public DateTime ChequeDate { get; set; }
        public string PaymentRefNo { get; set; }
        public string PaymentRefDate { get; set; }
        public int UserID { get; set; }

        [NotMapped]
        public string WorkflowStatus { get; set; }

        [NotMapped]
        public int WorkflowEscalationGroupID { get; set; }

        [NotMapped]
        [Display(Name = "Gst Amount")]
        public double TotalGstAmount { get; set; }

        [NotMapped]
        [Display(Name = "Amount After Gst")]
        public double TotalAmountAfterGst { get; set; }

        public virtual ICollection<PaymentLine> PaymentLine { get; set; }

        public virtual Vendor Vendor { get; set; } //Navigation Property to parent table Vendor


    }
}

