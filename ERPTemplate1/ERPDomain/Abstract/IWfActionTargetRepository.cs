using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfActionTargetRepository
    {
        IEnumerable<WfActionTarget> WfActionTarget { get; }
        void SaveWfActionTarget(WfActionTarget wfActionTarget);
        void DeleteWfActionTarget(WfActionTarget wfActionTarget);
    }
}
