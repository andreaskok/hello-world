using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfRequestActionRepository
    {
        IEnumerable<WfRequestAction> WfRequestAction { get; }
        void SaveWfRequestAction(WfRequestAction wfRequestAction);
        void DeleteWfRequestAction(WfRequestAction wfRequestAction);
    }
}
