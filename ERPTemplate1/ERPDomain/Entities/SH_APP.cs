using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class SH_APP
    {
        [Key]
        public int ID { get; set; }
        public int ModuleID { get; set; }
        public string FunctionName { get; set; }

        public virtual ICollection<SH_ROLEACCESS> SH_ROLEACCESS { get; set; }
    }
}
