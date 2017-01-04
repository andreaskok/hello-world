using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfActionTypeRepository
    {
        IEnumerable<WfActionType> WfActionType { get; }
        void SaveWfActionType(WfActionType wfActionType);
        void DeleteWfActionType(WfActionType wfActionType);
    }
}
