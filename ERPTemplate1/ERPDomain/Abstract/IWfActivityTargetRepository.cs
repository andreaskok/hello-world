using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfActivityTargetRepository
    {
        IEnumerable<WfActivityTarget> WfActivityTarget { get; }
        void SaveWfActivityTarget(WfActivityTarget wfActivityTarget);
        void DeleteWfActivityTarget(WfActivityTarget wfActivityTarget);
    }
}
