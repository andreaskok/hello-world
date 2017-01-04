using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class InvoiceLine
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 InvoiceID { get; set; } // Foreign Key 
        public Int64 ChartOfAccountID { get; set; } // Foreign Key 
        public Int64 CurrencyID { get; set; } // Foreign Key 
        public Int64 TaxCodeID { get; set; } // Foreign Key 

        public string InvoiceLineCode { get; set; }
        public string InvoiceCode { get; set; }
        public string AccCode { get; set; }
        public string BlkCode { get; set; }
        public string VehCode { get; set; }
        public string VehExpenseCode { get; set; }
        public string Description { get; set; }
        public Double Quantity { get; set; }
        public Double UnitPrice { get; set; }
        public Double Total { get; set; }
        public string LineInd { get; set; }
        public string DisRef { get; set; }
        public string Discnt { get; set; }
        public string DisInd { get; set; }
        public string DisType { get; set; }
        public string CurrencyCost { get; set; }
        public Double CurrencyAmount { get; set; }
        public Double TaxRate { get; set; }
        public string TaxRef { get; set; }
        public string TaxInd { get; set; }

        //Foreign Table
        public virtual Invoice Invoice { get; set; } //Navigation Property to parent table Invoice
        public virtual ChartOfAccount ChartOfAccount { get; set; } //Navigation Property to parent table ChartOfAccount
    }
}
