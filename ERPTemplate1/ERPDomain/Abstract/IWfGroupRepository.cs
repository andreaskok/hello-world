using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfGroupRepository
    {
        IEnumerable<WfGroup> WfGroup { get; }
        void SaveWfGroup(WfGroup wfGroup);
        void DeleteWfGroup(WfGroup wfGroup);
    }
}
