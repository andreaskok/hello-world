using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERPDomain.Entities
{
    public class DebitNote
    {
        [Key]
        public Int64 ID { get; set; }

        [Required(ErrorMessage = "Please enter debit note code")]
        public string DebitNoteCode { get; set; }
        public string DocType { get; set; }
        public string CustomerCode { get; set; }
        public Double TotalAmount { get; set; }
        #region "Group and hide other fields to show navigation property for documentation purpose"
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

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]

        [Required(ErrorMessage = "Please enter debit note date")]
        public DateTime DebitNoteDate { get; set; }
        public string InvoiceCode { get; set; }
        #endregion
        public virtual ICollection<DebitNoteLine> DebitNoteLine { get; set; }

    }
}
