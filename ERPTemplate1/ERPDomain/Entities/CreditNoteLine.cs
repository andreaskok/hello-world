using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class CreditNoteLine
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 CreditNoteID { get; set; }
        public Int64 ChartOfAccountID { get; set; }
        public string CreditNoteLineCode { get; set; }
        public string CreditNoteCode { get; set; }
        public string AccCode { get; set; }
        public string BlkCode { get; set; }
        public string VehCode { get; set; }
        public string VehExpenseCode { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public Double UnitPrice { get; set; }
        public Double Total { get; set; }
        public Double CurrencyCost { get; set; }
        public Double CurrencyAmount { get; set; }
        public Double TaxRate { get; set; }
        public string TaxRef { get; set; }
        public string TaxInd { get; set; }

        public virtual CreditNote CreditNote { get; set; }
        public virtual ChartOfAccount ChartOfAccount { get; set; }

    }
}
