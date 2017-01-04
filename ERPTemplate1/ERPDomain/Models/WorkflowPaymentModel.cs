using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Models
{
    public class WorkflowPaymentModel
    {
        public Payment Payment { get; set; }
        public IEnumerable<WfGroupMember> WfGroupMember { get; set; }
    }
}
