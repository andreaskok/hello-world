using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IWfRequestRepository
    {
        IEnumerable<WfRequest> WfRequest { get; }
        void SaveWfRequest(WfRequest wfRequest);
        void DeleteWfRequest(WfRequest wfRequest);
    }
}
