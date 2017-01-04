using System;
using System.ComponentModel.DataAnnotations;

namespace ERPDomain.Entities
{
    public class DebitNoteLine
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 DebitNoteID { get; set; }
        public Int64 ChartOfAccountID { get; set; }
        public string DebitNoteLineCode { get; set; }
        #region "Group and hide other fields to show navigation property for documentation purpose"
        public string DebitNoteCode { get; set; }
        public string AccCode { get; set; }
        public string BlkCode { get; set; }
        public string VehCode { get; set; }
        public string VehExpenseCode { get; set; }
        #endregion
        public string Description { get; set; }
        public Double Quantity { get; set; }
        public Double UnitPrice { get; set; }
        public Double Total { get; set; }
        #region "Group and hide other fields to show navigation property for documentation purpose"
        public Double CurrencyCost { get; set; }
        public Double CurrencyAmount { get; set; }
        public Double TaxRate { get; set; }
        public string TaxRef { get; set; }
        public string TaxInd { get; set; }
        #endregion
        public virtual DebitNote DebitNote { get; set; }
        public virtual ChartOfAccount ChartOfAccount { get; set; }

    }
}

