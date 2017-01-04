using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class CountryState
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 CountryID { get; set; }
        public string StateName { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<Calendar> Calendar { get; set; }
    }
}
