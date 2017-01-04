using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class UserPreference
    {
        [Key]
        public Int64 ID { get; set; }

        public Int64 SH_USERID { get; set; } //Foreign Key
        public String Language { get; set; }
        public String Theme { get; set; }
        public String RowPerPage { get; set; }
        public String DateFormat { get; set; }
        //public virtual SH_USER SH_USER { get; set; } //Navigation Property to parent table SH_USER

    }
}
