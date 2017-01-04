using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Country
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 CurrencyID { get; set; }
        public String CountryCode { get; set; }
        public String Description { get; set; }
        public String CountryISO3Code { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual ICollection<CountryState> CountryState { get; set; }

    }
}
