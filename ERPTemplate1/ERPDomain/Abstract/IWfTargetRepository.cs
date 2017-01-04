using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfTargetRepository
    {
        IEnumerable<WfTarget> WfTarget { get; }
        void SaveWfTarget(WfTarget wfTarget);
        void DeleteWfTarget(WfTarget wfTarget);
    }
}
