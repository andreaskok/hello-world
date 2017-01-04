using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfTarget
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<WfActionTarget> WfActionTarget { get; set; } //Navigation property to child table WfActionTarget
        public virtual ICollection<WfActivityTarget> WfActivityTarget { get; set; } //Navigation property to child table WfActivityTarget
    }
}
