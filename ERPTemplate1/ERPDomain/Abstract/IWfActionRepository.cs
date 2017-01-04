using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfActionRepository
    {
        IEnumerable<WfAction> WfAction { get; }
        void SaveWfAction(WfAction wfAction);
        void DeleteWfAction(WfAction wfAction);
    }
}
