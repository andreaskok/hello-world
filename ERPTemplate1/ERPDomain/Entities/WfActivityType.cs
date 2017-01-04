using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfActivityType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<WfActivity> WfActivity { get; set; } //Navigation to child table WfActivity
    }
}
