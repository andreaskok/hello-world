using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDomain.Entities
{
    public class WfStateType
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<WfState> WfState { get; set; }
    }
}
