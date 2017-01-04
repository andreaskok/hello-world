using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfProcessRepository
    {
        IEnumerable<WfProcess> WfProcess { get; }
        void SaveWfProcess(WfProcess wfProcess);
        void DeleteWfProcess(WfProcess wfProcess);
    }
}
