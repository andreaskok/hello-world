using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class JournalLine
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 JournalID { get; set; } //Foreign Key
        public Int64 ChartOfAccountID { get; set; } //Foreign Key
        public Int64 TaxCodeID { get; set; } //Foreign Key

        public string JournalLineCode { get; set; }
        public string JournalCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public Double UnitPrice { get; set; }
        public Double Total { get; set; }
        public string AccCode { get; set; }
        public string BlkCode { get; set; }
        public string VehCode { get; set; }
        public string VehExpCode { get; set; }
        public string LocCode { get; set; }
        public string ItemCode { get; set; }
        public string AutoGenRecords { get; set; }
        public string CreditInd { get; set; }
        public string SourceLineNo { get; set; }
        public string TargetLineNo { get; set; }
        public string ExportTO { get; set; }
        public Double TaxRate { get; set; }
        public string TaxRef { get; set; }
        public string TaxInd { get; set; }

        [Display(Name = "DebitOrCredit")]
        public string DebitCreditIndicator { get; set; }

        //Foreign Table
        public virtual Journal Journal { get; set; } //Navigation Property to parent table Journal
        public virtual ChartOfAccount ChartOfAccount { get; set; } //Navigation Property to parent table ChartOfAccount
                                                                   //public virtual ICollection<ChartOfAccount> ChartOfAccount { get; set; }
                                                                   //public virtual ICollection<Tax> Tax { get; set; }

    }
}

