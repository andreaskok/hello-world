using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class PaymentLine
    {

        [Key]
        public Int64 ID { get; set; }

        public Int64 PaymentID { get; set; } // Foreign Key 
        public Int64 ChartOfAccountID { get; set; } // Foreign Key 
        public Int64 CurrencyID { get; set; } // Foreign Key 
        public string PaymentLineCode { get; set; }
        public string PaymentCode { get; set; }
        public string ItemCode { get; set; }
        public string DocType { get; set; }
        public string AccCode { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.######}")]
        public Double Amount { get; set; }
        public Decimal CurrencyAmount { get; set; }
        public Decimal PrevCurrRate { get; set; }
        public Decimal PrevCurrAmount { get; set; }
        public Decimal PrevAmount { get; set; }

        [NotMapped]
        public Double GstPct { get; set; }

        [NotMapped]
        public Double GstAmount { get; set; }

        [NotMapped]
        public Double AmountAfterGst { get; set; }

        //Foreign Table
        public virtual Payment Payment { get; set; } //Navigation Property to parent table Payment
        public virtual ChartOfAccount ChartOfAccount { get; set; } //Navigation Property to parent table ChartOfAccount

    }
}
