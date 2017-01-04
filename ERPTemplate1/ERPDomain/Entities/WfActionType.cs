using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfActionType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<WfAction> WfAction { get; set; } //Navigation to Child table WfAction
    }
}
