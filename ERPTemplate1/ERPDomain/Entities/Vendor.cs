using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Vendor
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 ChartOfAccountID { get; set; } //Foreign Key
        public Int64 BankID { get; set; } //Foreign Key
        public Int64 CurrencyID { get; set; } //Foreign Key
        public Int64 TaxCodeID { get; set; } //Foreign Key

        public string VendorCode { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public string TelNo { get; set; }
        public string FaxNo { get; set; }
        public string Email { get; set; }
        public string FinAccCode { get; set; }
        public string AccCode { get; set; }
        public string CreditTerm { get; set; }
        public string TermType { get; set; }
        public Decimal CreditLimit { get; set; }
        public string BankCode { get; set; }
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateID { get; set; }
        public string SuppType { get; set; }
        public string TermCond { get; set; }
        public string MobileTel { get; set; }
        public string CurrencyCode { get; set; }
        public string ComRegisterNum { get; set; }
        public string SuppBRN { get; set; }
        public string SuppGSTNo { get; set; }
        public DateTime DateGST { get; set; }
        public string TaxCode { get; set; }
        public string FinSupplierCode { get; set; }
        public string IsEnabledSelfBilled { get; set; }
        public string RMCDApproveNo { get; set; }

        public virtual ChartOfAccount ChartOfAccount { get; set; }
        //public virtual Payment Payment { get; set; }


    }
}
