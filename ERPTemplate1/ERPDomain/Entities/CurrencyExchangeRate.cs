using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class CurrencyExchangeRate
    {
        [Key]
        public Int64 ID { get; set; }
        public String ForeignCurrencyCode { get; set; }
        public String BaseCurrencyCode { get; set; }
        public Double ExchangeRate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public String UpdateID { get; set; }

        //public virtual ICollection<Currency> Currency { get; set; }
    }

}
