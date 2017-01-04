using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class Currency
    {
        [Key]
        public Int64 ID { get; set; }

        public String CurrencyCode { get; set; }
        public String Description { get; set; }

        public virtual ICollection<Country> Country { get; set; }
    }
}
