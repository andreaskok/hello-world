using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfStateRepository
    {
        IEnumerable<WfState> WfState { get; }
        void SaveWfState(WfState wfState);
        void DeleteWfState(WfState wfState);
    }
}
